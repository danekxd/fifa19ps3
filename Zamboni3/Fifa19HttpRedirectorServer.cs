using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using NLog;

namespace Zamboni14Legacy;

/// <summary>
/// FIFA 19 PS3 uses HTTPS + XML for the redirector request on port 42230.
/// It does not send a binary Blaze FireFrame on this first connection.
/// </summary>
internal sealed class Fifa19HttpRedirectorServer
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly int _port;
    private readonly X509Certificate2 _certificate;
    private readonly string _gameServerIp;
    private readonly int _gameServerPort;

    public Fifa19HttpRedirectorServer(
        int port,
        X509Certificate2 certificate,
        string gameServerIp,
        int gameServerPort)
    {
        _port = port;
        _certificate = certificate;
        _gameServerIp = gameServerIp;
        _gameServerPort = gameServerPort;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var listener = new TcpListener(IPAddress.Any, _port);
        listener.Start();

        Logger.Info(
            "FIFA 19 HTTPS/XML redirector started on port {Port}.",
            _port);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient client =
                    await listener.AcceptTcpClientAsync(cancellationToken)
                        .ConfigureAwait(false);

                _ = HandleClientAsync(client, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Normal shutdown.
        }
        finally
        {
            listener.Stop();
        }
    }

    private async Task HandleClientAsync(
        TcpClient client,
        CancellationToken cancellationToken)
    {
        string remote =
            client.Client.RemoteEndPoint?.ToString() ?? "unknown";

        using (client)
        using (var ssl = new SslStream(
                   client.GetStream(),
                   leaveInnerStreamOpen: false))
        {
            try
            {
                Logger.Info(
                    "FIFA redirector connection accepted from {Remote}.",
                    remote);

                await ssl.AuthenticateAsServerAsync(
        _certificate,
        clientCertificateRequired: false,
        enabledSslProtocols: SslProtocols.None,
        checkCertificateRevocation: false)
    .ConfigureAwait(false);

                Logger.Info(
                    "FIFA redirector TLS authenticated for {Remote}.",
                    remote);

                HttpRequest request =
                    await ReadHttpRequestAsync(
                            ssl,
                            cancellationToken)
                        .ConfigureAwait(false);

                Logger.Warn(
                    "FIFA redirector HTTP request: {Method} {Path}, body={Length} bytes.",
                    request.Method,
                    request.Path,
                    request.Body.Length);

                LogSafeRequestMetadata(request.Body);

                if (!request.Method.Equals(
                        "POST",
                        StringComparison.OrdinalIgnoreCase) ||
                    !request.Path.Equals(
                        "/redirector/getServerInstance",
                        StringComparison.Ordinal))
                {
                    await WriteSimpleResponseAsync(
                            ssl,
                            "404 Not Found",
                            "text/plain",
                            "Not Found",
                            cancellationToken)
                        .ConfigureAwait(false);
                    return;
                }

                byte[] ipBytes =
                    IPAddress.Parse(_gameServerIp).GetAddressBytes();

                if (ipBytes.Length != 4)
                {
                    throw new InvalidOperationException(
                        "GameServerIp must be an IPv4 address.");
                }

                uint ipNumber =
                    ((uint)ipBytes[0] << 24) |
                    ((uint)ipBytes[1] << 16) |
                    ((uint)ipBytes[2] << 8) |
                    ipBytes[3];

                // CoreServer is currently plaintext, so secure must be 0.
                string responseXml =
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                    "<serverinstanceinfo>\n" +
                    "  <address member=\"0\">\n" +
                    "    <valu>\n" +
                    $"      <hostname>{_gameServerIp}</hostname>\n" +
                    $"      <ip>{ipNumber}</ip>\n" +
                    $"      <port>{_gameServerPort}</port>\n" +
                    "    </valu>\n" +
                    "  </address>\n" +
                    "  <secure>0</secure>\n" +
                    "  <defaultdnsaddress>0</defaultdnsaddress>\n" +
                    "</serverinstanceinfo>\n";

                byte[] body = Encoding.UTF8.GetBytes(responseXml);

                string headers =
                    "HTTP/1.1 200 OK\r\n" +
                    "Content-Type: text/xml\r\n" +
                    "X-BLAZE-COMPONENT: redirector\r\n" +
                    "X-BLAZE-COMMAND: getServerInstance\r\n" +
                    "X-BLAZE-SEQNO: 0\r\n" +
                    $"Content-Length: {body.Length}\r\n" +
                    "Connection: close\r\n" +
                    "\r\n";

                byte[] headerBytes = Encoding.ASCII.GetBytes(headers);

                await ssl.WriteAsync(
                        headerBytes.AsMemory(),
                        cancellationToken)
                    .ConfigureAwait(false);

                await ssl.WriteAsync(
                        body.AsMemory(),
                        cancellationToken)
                    .ConfigureAwait(false);

                await ssl.FlushAsync(cancellationToken)
                    .ConfigureAwait(false);

                Logger.Warn(
                    "Returned FIFA core endpoint {Ip}:{Port}, secure=0.",
                    _gameServerIp,
                    _gameServerPort);
            }
            catch (Exception exception)
            {
                Logger.Error(
                    exception,
                    "FIFA redirector connection failed for {Remote}.",
                    remote);
            }
        }
    }

    private static async Task<HttpRequest> ReadHttpRequestAsync(
        Stream stream,
        CancellationToken cancellationToken)
    {
        const int maximumRequestSize = 64 * 1024;

        using var buffer = new MemoryStream();
        byte[] chunk = new byte[4096];

        int headerEnd = -1;
        int contentLength = 0;

        while (true)
        {
            int read = await stream.ReadAsync(
                    chunk.AsMemory(),
                    cancellationToken)
                .ConfigureAwait(false);

            if (read == 0)
            {
                throw new IOException(
                    "Client closed before the complete HTTP request was read.");
            }

            buffer.Write(chunk, 0, read);

            if (buffer.Length > maximumRequestSize)
            {
                throw new InvalidDataException(
                    "HTTP request exceeded the size limit.");
            }

            byte[] data = buffer.ToArray();

            if (headerEnd < 0)
            {
                headerEnd = FindHeaderEnd(data);

                if (headerEnd >= 0)
                {
                    string headerText =
                        Encoding.ASCII.GetString(data, 0, headerEnd);

                    contentLength =
                        ParseContentLength(headerText);
                }
            }

            if (headerEnd >= 0 &&
                data.Length >= headerEnd + 4 + contentLength)
            {
                string headerText =
                    Encoding.ASCII.GetString(data, 0, headerEnd);

                string[] lines = headerText.Split(
                    "\r\n",
                    StringSplitOptions.None);

                string[] requestLine = lines[0].Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries);

                if (requestLine.Length < 2)
                {
                    throw new InvalidDataException(
                        "Invalid HTTP request line.");
                }

                byte[] body = data
                    .Skip(headerEnd + 4)
                    .Take(contentLength)
                    .ToArray();

                return new HttpRequest(
                    requestLine[0],
                    requestLine[1],
                    body);
            }
        }
    }

    private static int FindHeaderEnd(byte[] data)
    {
        for (int index = 0; index <= data.Length - 4; index++)
        {
            if (data[index] == 13 &&
                data[index + 1] == 10 &&
                data[index + 2] == 13 &&
                data[index + 3] == 10)
            {
                return index;
            }
        }

        return -1;
    }

    private static int ParseContentLength(string headers)
    {
        foreach (string line in headers.Split(
                     "\r\n",
                     StringSplitOptions.None))
        {
            int separator = line.IndexOf(':');

            if (separator <= 0)
            {
                continue;
            }

            string name = line[..separator].Trim();

            if (!name.Equals(
                    "Content-Length",
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string value = line[(separator + 1)..].Trim();

            if (int.TryParse(value, out int length) &&
                length >= 0)
            {
                return length;
            }

            throw new InvalidDataException(
                "Invalid Content-Length header.");
        }

        return 0;
    }

    private static void LogSafeRequestMetadata(byte[] body)
    {
        try
        {
            var document = XDocument.Parse(
                Encoding.UTF8.GetString(body));

            XElement? root = document.Root;

            Logger.Info(
                "Client={Client}, platform={Platform}, BlazeSDK={Sdk}, profile={Profile}.",
                root?.Element("clientname")?.Value ?? "?",
                root?.Element("clientplatform")?.Value ?? "?",
                root?.Element("blazesdkversion")?.Value ?? "?",
                root?.Element("connectionprofile")?.Value ?? "?");
        }
        catch (Exception exception)
        {
            Logger.Warn(
                exception,
                "Could not parse redirector XML metadata.");
        }
    }

    private static async Task WriteSimpleResponseAsync(
        Stream stream,
        string status,
        string contentType,
        string text,
        CancellationToken cancellationToken)
    {
        byte[] body = Encoding.UTF8.GetBytes(text);

        string headers =
            $"HTTP/1.1 {status}\r\n" +
            $"Content-Type: {contentType}\r\n" +
            $"Content-Length: {body.Length}\r\n" +
            "Connection: close\r\n" +
            "\r\n";

        await stream.WriteAsync(
                Encoding.ASCII.GetBytes(headers).AsMemory(),
                cancellationToken)
            .ConfigureAwait(false);

        await stream.WriteAsync(
                body.AsMemory(),
                cancellationToken)
            .ConfigureAwait(false);

        await stream.FlushAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private sealed record HttpRequest(
        string Method,
        string Path,
        byte[] Body);
}
