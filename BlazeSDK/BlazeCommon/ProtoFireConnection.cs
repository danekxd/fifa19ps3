using Org.Mentalis.Security.Ssl;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace BlazeCommon
{
    public class ProtoFireConnection
    {
        private const int Fire2HeaderSize = 16;
        private const uint MaximumPayloadSize = 16 * 1024 * 1024;
        private const ushort MaximumMetadataSize = ushort.MaxValue;

        /// <summary>
        /// The framing assigned to newly accepted connections.
        ///
        /// Keep LegacyFireFrame as the library default so existing Zamboni
        /// titles are not silently changed. FIFA 19 must set this to Fire2
        /// before the core ProtoFireServer starts.
        /// </summary>
        public static ProtoFireFraming DefaultFraming { get; set; } =
            ProtoFireFraming.LegacyFireFrame;

        public long ID { get; }
        public ProtoFireServer? Owner { get; }
        public Socket Socket { get; }
        public Stream? Stream { get; private set; }
        public bool Connected { get; private set; }

        public ProtoFireFraming Framing { get; set; }

        private static readonly SemaphoreSlim Semaphore =
            new SemaphoreSlim(1, 1);

        public ProtoFireConnection(
            long id,
            ProtoFireServer owner,
            Socket socket)
        {
            ID = id;
            Owner = owner;
            Socket = socket;
            Stream = null;
            Connected = true;
            Framing = DefaultFraming;
        }

        public ProtoFireConnection(Socket socket)
        {
            ID = 0;
            Owner = null;
            Socket = socket;
            Stream = null;
            Connected = true;
            Framing = DefaultFraming;
        }

        public void SetStream(Stream stream)
        {
            if (Stream != null)
                throw new InvalidOperationException(
                    "Stream is already set");

            Stream = stream;
        }

        public void Disconnect()
        {
            if (!Connected)
                return;

            Connected = false;

            // Stream owns the socket, so no separate socket close is needed.
            try
            {
                Stream?.Close();
            }
            catch
            {
                // Best-effort disconnect.
            }

            Owner?.KillConnection(this);
        }

        public async Task<ProtoFirePacket?> ReadPacketAsync()
        {
            if (!Connected)
                return null;

            if (Stream == null)
                throw new InvalidOperationException(
                    "Stream is not set");

            try
            {
                return Framing == ProtoFireFraming.Fire2
                    ? await ReadFire2PacketAsync(Stream)
                        .ConfigureAwait(false)
                    : await ReadLegacyPacketAsync(Stream)
                        .ConfigureAwait(false);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ProtoFirePacket? ReadPacket()
        {
            if (!Connected)
                return null;

            if (Stream == null)
                throw new InvalidOperationException(
                    "Stream is not set");

            try
            {
                return Framing == ProtoFireFraming.Fire2
                    ? ReadFire2Packet(Stream)
                    : ReadLegacyPacket(Stream);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static async Task<ProtoFirePacket?>
            ReadLegacyPacketAsync(Stream stream)
        {
            FireFrame frame = new FireFrame();

            if (!await stream.ReadAllAsync(
                    frame.Frame,
                    0,
                    FireFrame.MIN_HEADER_SIZE)
                .ConfigureAwait(false))
            {
                return null;
            }

            ushort extraFrameBytesNeeded =
                frame.ExtraHeaderSize;

            if (!await stream.ReadAllAsync(
                    frame.Frame,
                    FireFrame.MIN_HEADER_SIZE,
                    extraFrameBytesNeeded)
                .ConfigureAwait(false))
            {
                return null;
            }

            uint payloadSize = frame.Size;
            ValidatePayloadSize(payloadSize);

            byte[] data = new byte[checked((int)payloadSize)];

            if (!await stream.ReadAllAsync(
                    data,
                    0,
                    data.Length)
                .ConfigureAwait(false))
            {
                return null;
            }

            return new ProtoFirePacket(
                frame,
                data,
                ProtoFireFraming.LegacyFireFrame);
        }

        private static ProtoFirePacket?
            ReadLegacyPacket(Stream stream)
        {
            FireFrame frame = new FireFrame();

            if (!stream.ReadAll(
                    frame.Frame,
                    0,
                    FireFrame.MIN_HEADER_SIZE))
            {
                return null;
            }

            ushort extraFrameBytesNeeded =
                frame.ExtraHeaderSize;

            if (!stream.ReadAll(
                    frame.Frame,
                    FireFrame.MIN_HEADER_SIZE,
                    extraFrameBytesNeeded))
            {
                return null;
            }

            uint payloadSize = frame.Size;
            ValidatePayloadSize(payloadSize);

            byte[] data = new byte[checked((int)payloadSize)];

            if (!stream.ReadAll(data, 0, data.Length))
                return null;

            return new ProtoFirePacket(
                frame,
                data,
                ProtoFireFraming.LegacyFireFrame);
        }

        private static async Task<ProtoFirePacket?>
            ReadFire2PacketAsync(Stream stream)
        {
            byte[] header = new byte[Fire2HeaderSize];

            if (!await stream.ReadAllAsync(
                    header,
                    0,
                    header.Length)
                .ConfigureAwait(false))
            {
                return null;
            }

            Fire2Header parsed = ParseFire2Header(header);

            byte[] metadata =
                new byte[parsed.MetadataSize];

            if (!await stream.ReadAllAsync(
                    metadata,
                    0,
                    metadata.Length)
                .ConfigureAwait(false))
            {
                return null;
            }

            byte[] data =
                new byte[checked((int)parsed.PayloadSize)];

            if (!await stream.ReadAllAsync(
                    data,
                    0,
                    data.Length)
                .ConfigureAwait(false))
            {
                return null;
            }

            return CreateFire2Packet(parsed, metadata, data);
        }

        private static ProtoFirePacket?
            ReadFire2Packet(Stream stream)
        {
            byte[] header = new byte[Fire2HeaderSize];

            if (!stream.ReadAll(
                    header,
                    0,
                    header.Length))
            {
                return null;
            }

            Fire2Header parsed = ParseFire2Header(header);

            byte[] metadata =
                new byte[parsed.MetadataSize];

            if (!stream.ReadAll(
                    metadata,
                    0,
                    metadata.Length))
            {
                return null;
            }

            byte[] data =
                new byte[checked((int)parsed.PayloadSize)];

            if (!stream.ReadAll(data, 0, data.Length))
                return null;

            return CreateFire2Packet(parsed, metadata, data);
        }

        private static Fire2Header ParseFire2Header(
            byte[] header)
        {
            uint payloadSize =
                ReadUInt32BigEndian(header, 0);

            ushort metadataSize =
                ReadUInt16BigEndian(header, 4);

            ValidatePayloadSize(payloadSize);

            if (metadataSize > MaximumMetadataSize)
            {
                throw new InvalidDataException(
                    "Fire2 metadata is too large.");
            }

            ushort component =
                ReadUInt16BigEndian(header, 6);

            ushort command =
                ReadUInt16BigEndian(header, 8);

            uint messageNumber =
                ((uint)header[10] << 16) |
                ((uint)header[11] << 8) |
                header[12];

            byte typeAndUserIndex = header[13];
            byte messageType =
                (byte)(typeAndUserIndex >> 5);

            byte userIndex =
                (byte)(typeAndUserIndex & 0x1F);

            return new Fire2Header(
                payloadSize,
                metadataSize,
                component,
                command,
                messageNumber,
                messageType,
                userIndex,
                header[14],
                header[15]);
        }

        private static ProtoFirePacket CreateFire2Packet(
            Fire2Header parsed,
            byte[] metadata,
            byte[] data)
        {
            FireFrame frame = new FireFrame
            {
                Size = parsed.PayloadSize,
                Component = parsed.Component,
                Command = parsed.Command,
                MsgNum = parsed.MessageNumber,
                MsgType =
                    (FireFrame.MessageType)parsed.MessageType,

                // The legacy FireFrame class stores four user-index bits.
                // FIFA's observed packets use index zero, so this conversion
                // is lossless for the current client.
                UserIndex =
                    (byte)(parsed.UserIndex & 0x0F)
            };

            return new ProtoFirePacket(
                frame,
                data,
                ProtoFireFraming.Fire2,
                metadata,
                parsed.Options,
                parsed.Reserved);
        }

        private static void ValidatePayloadSize(
            uint payloadSize)
        {
            if (payloadSize > MaximumPayloadSize)
            {
                throw new InvalidDataException(
                    $"ProtoFire payload is too large: " +
                    $"{payloadSize} bytes.");
            }
        }

        private static ushort ReadUInt16BigEndian(
            byte[] buffer,
            int offset)
        {
            return (ushort)(
                (buffer[offset] << 8) |
                buffer[offset + 1]);
        }

        private static uint ReadUInt32BigEndian(
            byte[] buffer,
            int offset)
        {
            return
                ((uint)buffer[offset] << 24) |
                ((uint)buffer[offset + 1] << 16) |
                ((uint)buffer[offset + 2] << 8) |
                buffer[offset + 3];
        }

        private readonly struct Fire2Header
        {
            public uint PayloadSize { get; }
            public ushort MetadataSize { get; }
            public ushort Component { get; }
            public ushort Command { get; }
            public uint MessageNumber { get; }
            public byte MessageType { get; }
            public byte UserIndex { get; }
            public byte Options { get; }
            public byte Reserved { get; }

            public Fire2Header(
                uint payloadSize,
                ushort metadataSize,
                ushort component,
                ushort command,
                uint messageNumber,
                byte messageType,
                byte userIndex,
                byte options,
                byte reserved)
            {
                PayloadSize = payloadSize;
                MetadataSize = metadataSize;
                Component = component;
                Command = command;
                MessageNumber = messageNumber;
                MessageType = messageType;
                UserIndex = userIndex;
                Options = options;
                Reserved = reserved;
            }
        }

        public bool Send(ProtoFirePacket packet)
        {
            if (!Connected)
                return false;

            if (Stream == null)
                throw new InvalidOperationException(
                    "Stream is not set");

            bool success = false;
            Semaphore.Wait();

            try
            {
                packet.WriteTo(Stream, Framing);
                Stream.Flush();
                success = true;
            }
            catch (ObjectDisposedException)
            {
                success = false;
            }
            catch (IOException)
            {
                success = false;
            }
            finally
            {
                Semaphore.Release();
            }

            return success;
        }

        public async Task<bool> SendAsync(
            ProtoFirePacket packet)
        {
            if (!Connected)
                return false;

            if (Stream == null)
                throw new InvalidOperationException(
                    "Stream is not set");

            bool success = false;
            await Semaphore.WaitAsync().ConfigureAwait(false);

            try
            {
                await packet.WriteToAsync(Stream, Framing)
                    .ConfigureAwait(false);

                await Stream.FlushAsync()
                    .ConfigureAwait(false);

                success = true;
            }
            catch (ObjectDisposedException)
            {
                success = false;
            }
            catch (IOException)
            {
                success = false;
            }
            finally
            {
                Semaphore.Release();
            }

            return success;
        }

        private static async Task<Socket?>
            ConnectToAsync(
                string hostname,
                int port)
        {
            IPHostEntry host =
                Dns.GetHostEntry(hostname);

            if (host.AddressList.Length == 0)
                return null;

            IPAddress ipAddress =
                host.AddressList[0];

            IPEndPoint remoteEndpoint =
                new IPEndPoint(ipAddress, port);

            Socket socket = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            try
            {
                await socket.ConnectAsync(remoteEndpoint)
                    .ConfigureAwait(false);

                return socket;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<ProtoFireConnection?>
            ConnectAsync(
                string hostname,
                int port,
                bool ssl = true)
        {
            Socket? socket =
                await ConnectToAsync(hostname, port)
                    .ConfigureAwait(false);

            if (socket == null)
                return null;

            Stream stream =
                new NetworkStream(socket, ownsSocket: true);

            if (ssl)
            {
                SslStream sslStream = new SslStream(
                    stream,
                    leaveInnerStreamOpen: false,
                    RemoteCertificateVerify);

                await sslStream.AuthenticateAsClientAsync(
                        hostname,
                        null,
                        System.Security.Authentication
                            .SslProtocols.Tls,
                        checkCertificateRevocation: false)
                    .ConfigureAwait(false);

                stream = sslStream;
            }

            ProtoFireConnection connection =
                new ProtoFireConnection(socket);

            connection.SetStream(stream);
            return connection;
        }

        public static ProtoFireConnection?
            ConnectSsl3(
                string hostname,
                int port)
        {
            IPHostEntry host =
                Dns.GetHostEntry(hostname);

            if (host.AddressList.Length == 0)
                return null;

            SecurityOptions options = new SecurityOptions(
                SecureProtocol.Ssl3 | SecureProtocol.Tls1,
                null!,
                ConnectionEnd.Client,
                CredentialVerification.None,
                null!,
                hostname,
                SecurityFlags.Default,
                SslAlgorithms.ALL,
                null!);

            SecureSocket socket = new SecureSocket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp,
                options);

            socket.Connect(
                new IPEndPoint(host.AddressList[0], port));

            ProtoFireConnection connection =
                new ProtoFireConnection(null!);

            connection.SetStream(
                new SecureNetworkStream(
                    socket,
                    true));

            return connection;
        }

        public static ProtoFireConnection?
            ConnectSsl3(
                long address,
                int port)
        {
            SecurityOptions options = new SecurityOptions(
                SecureProtocol.Ssl3 | SecureProtocol.Tls1,
                null!,
                ConnectionEnd.Client,
                CredentialVerification.None,
                null!,
                null!,
                SecurityFlags.Default,
                SslAlgorithms.SECURE_CIPHERS,
                null!);

            SecureSocket socket = new SecureSocket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp,
                options);

            socket.Connect(
                new IPEndPoint(address, port));

            ProtoFireConnection connection =
                new ProtoFireConnection(null!);

            connection.SetStream(
                new SecureNetworkStream(
                    socket,
                    true));

            return connection;
        }

        private static bool RemoteCertificateVerify(
            object sender,
            X509Certificate? certificate,
            X509Chain? chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
