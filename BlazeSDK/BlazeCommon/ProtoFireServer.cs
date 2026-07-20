using FixedSsl;
using NLog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace BlazeCommon
{
    public abstract class ProtoFireServer
    {
        public string Name { get; private set; }
        public IPEndPoint LocalEP { get; private set; }
        public bool IsRunning { get; private set; }
        public X509Certificate? Certificate { get; private set; }
        public bool ForceSsl { get; private set; }

        [MemberNotNullWhen(true, nameof(Certificate))]
        public bool Secure => Certificate != null;

        private Socket? _listenSocket;
        private long _nextConnectionId;
        private readonly ConcurrentDictionary<long, ProtoFireConnection> _connections;
        private CancellationTokenSource _cancellationTokenSource;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected ProtoFireServer(
            string name,
            IPEndPoint localEP,
            X509Certificate? cert,
            bool forceSsl)
        {
            Name = name;
            LocalEP = localEP;
            IsRunning = false;
            Certificate = cert;
            ForceSsl = forceSsl;

            _connections = new ConcurrentDictionary<long, ProtoFireConnection>();
            _cancellationTokenSource = new CancellationTokenSource();
            _nextConnectionId = 0;
        }

        public void KillConnection(ProtoFireConnection connection)
        {
            if (connection.Connected)
            {
                connection.Disconnect();
                return;
            }

            OnProtoFireDisconnectInternalAsync(connection)
                .GetAwaiter()
                .GetResult();
        }

        public void Stop()
        {
            IsRunning = false;
            _cancellationTokenSource.Cancel();
        }

        public async Task Start(int backlog)
        {
            if (IsRunning)
                return;

            if (_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
            }

            try
            {
                _logger.Info(
                    "Starting {Security} ProtoFireServer({Name}) on port {Port}...",
                    Secure ? "secure" : "insecure",
                    Name,
                    LocalEP.Port);

                _listenSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);

                _listenSocket.Bind(LocalEP);
                _listenSocket.Listen(backlog);

                IsRunning = true;

                _logger.Info(
                    "ProtoFireServer({Name}) started.",
                    Name);
            }
            catch (Exception ex)
            {
                _logger.Error(
                    ex,
                    "Failed to start {Security} ProtoFireServer({Name}) on port {Port}. Perhaps the port is already in use.",
                    Secure ? "secure" : "insecure",
                    Name,
                    LocalEP.Port);

                IsRunning = false;
                return;
            }

            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Socket socket =
                        await _listenSocket.AcceptAsync(
                                _cancellationTokenSource.Token)
                            .ConfigureAwait(false);

                    Console.WriteLine(
                        $"[{DateTime.Now:HH:mm:ss.fff}] " +
                        $"Socket accepted, remote={socket.RemoteEndPoint}");

                    long clientId =
                        Interlocked.Increment(ref _nextConnectionId);

                    ProtoFireConnection connection =
                        new ProtoFireConnection(
                            clientId,
                            this,
                            socket);

                    // Do not let logging or connection callbacks delay
                    // the FIFA client's already-sent preAuth packet.
                    InitializeAcceptedConnection(connection);
                }
            }
            catch (OperationCanceledException)
            {
                // Normal shutdown.
            }
            catch (ObjectDisposedException)
            {
                // Listener was closed during shutdown.
            }
            finally
            {
                IsRunning = false;

                try
                {
                    _listenSocket?.Close();
                }
                catch
                {
                    // Best-effort shutdown.
                }

                _nextConnectionId = 0;

                foreach (ProtoFireConnection connection in _connections.Values)
                {
                    connection.Disconnect();
                }

                _connections.Clear();
            }
        }

        private void InitializeAcceptedConnection(
            ProtoFireConnection connection)
        {
            if (!_connections.TryAdd(
                    connection.ID,
                    connection))
            {
                connection.Disconnect();
                return;
            }

            if (!connection.Connected)
                return;

            if (Secure)
            {
                SslSocket.BeginAuthenticateAsServer(
                    connection.Socket,
                    Certificate,
                    ForceSsl,
                    AuthenticateAsServerCallback,
                    connection);

                _ = RunConnectCallbackAsync(connection);
                return;
            }

            // Plaintext core server:
            // attach the stream and begin reading before any potentially
            // blocking logging/callback work.
            connection.SetStream(
                new NetworkStream(
                    connection.Socket,
                    ownsSocket: true));

            _ = RunReceiveLoopAsync(connection);
            _ = RunConnectCallbackAsync(connection);
        }

        private async Task RunConnectCallbackAsync(
            ProtoFireConnection connection)
        {
            try
            {
                Console.WriteLine(
                    $"[{DateTime.Now:HH:mm:ss.fff}] " +
                    $"Connection({connection.ID}) accepted from " +
                    $"{connection.Socket.RemoteEndPoint}");

                await OnProtoFireConnectAsync(connection)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await OnProtoFireErrorInternalAsync(
                        connection,
                        ex)
                    .ConfigureAwait(false);
            }
        }

        private async void AuthenticateAsServerCallback(
            IAsyncResult result)
        {
            ProtoFireConnection connection =
                (ProtoFireConnection)result.AsyncState!;

            try
            {
                Stream? stream =
                    SslSocket.EndAuthenticateAsServer(result);

                if (stream == null)
                {
                    _logger.Info(
                        "Failed to authenticate as server for connection({ClientId}).",
                        connection.ID);

                    connection.Disconnect();
                    return;
                }

                connection.SetStream(stream);

                _logger.Info(
                    "Authenticated as server for connection({ClientId}). Stream type: {StreamType}",
                    connection.ID,
                    stream.GetType().Name);

                await RunReceiveLoopAsync(connection)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.Error(
                    ex,
                    "Failed to authenticate as server for connection({ClientId}).",
                    connection.ID);

                connection.Disconnect();
            }
        }

        private async Task RunReceiveLoopAsync(
            ProtoFireConnection connection)
        {
            try
            {
                while (IsRunning && connection.Connected)
                {
                    ProtoFirePacket? packet =
                        await connection.ReadPacketAsync()
                            .ConfigureAwait(false);

                    if (packet == null)
                        break;

                    try
                    {
                        await OnProtoFirePacketReceivedAsync(
                                connection,
                                packet)
                            .ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await OnProtoFireErrorInternalAsync(
                                connection,
                                ex)
                            .ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                connection.Disconnect();
            }
        }

        private async ValueTask OnProtoFireDisconnectInternalAsync(
            ProtoFireConnection connection)
        {
            if (!_connections.TryRemove(
                    connection.ID,
                    out _))
            {
                return;
            }

            _logger.Info(
                "Connection({ClientId}) disconnected.",
                connection.ID);

            try
            {
                await OnProtoFireDisconnectAsync(connection)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await OnProtoFireErrorInternalAsync(
                        connection,
                        ex)
                    .ConfigureAwait(false);
            }
        }

        private async Task OnProtoFireErrorInternalAsync(
            ProtoFireConnection connection,
            Exception exception)
        {
            try
            {
                await OnProtoFireErrorAsync(
                        connection,
                        exception)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.Error(
                    ex,
                    "An exception occurred while handling a ProtoFire error for connection({ClientId}).",
                    connection.ID);
            }
        }

        public abstract Task OnProtoFireConnectAsync(
            ProtoFireConnection connection);

        public abstract Task OnProtoFirePacketReceivedAsync(
            ProtoFireConnection connection,
            ProtoFirePacket packet);

        public abstract Task OnProtoFireDisconnectAsync(
            ProtoFireConnection connection);

        public abstract Task OnProtoFireErrorAsync(
            ProtoFireConnection connection,
            Exception exception);
    }
}