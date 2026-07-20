namespace Zamboni14Legacy;

public class Api
{
    private readonly string _address;

    public Api()
    {
        _address =
            "http://0.0.0.0:" +
            Program.ZamboniConfig.ApiServerPort;
    }

    public async Task StartAsync()
    {
        WebApplicationBuilder builder =
            WebApplication.CreateBuilder();

        WebApplication app = builder.Build();

        // Log every HTTP request that reaches the local FIFA API.
        // Sensitive authorization headers and PSN tickets are not printed.
        app.Use(async (context, next) =>
        {
            string remoteAddress =
                context.Connection.RemoteIpAddress?.ToString()
                ?? "unknown";

            Console.WriteLine(
                $"HTTP {context.Request.Method} " +
                $"{context.Request.Path}" +
                $"{context.Request.QueryString} " +
                $"from {remoteAddress}");

            await next();

            Console.WriteLine(
                $"HTTP response {context.Response.StatusCode} " +
                $"for {context.Request.Method} " +
                $"{context.Request.Path}");
        });

        app.MapGet("/connect/auth", async context =>
        {
            string clientId =
                context.Request.Query["client_id"]
                    .FirstOrDefault()
                ?? "";

            string responseType =
                context.Request.Query["response_type"]
                    .FirstOrDefault()
                ?? "";

            string redirectUri =
                context.Request.Query["redirect_uri"]
                    .FirstOrDefault()
                ?? "nucleus:rest";

            bool hasAuthorization =
                context.Request.Headers
                    .ContainsKey("Authorization");

            Console.WriteLine(
                "FIFA identity request: " +
                $"client_id={clientId}, " +
                $"response_type={responseType}, " +
                $"redirect_uri={redirectUri}, " +
                $"hasAuthorization={hasAuthorization}");

            // Never print the Authorization header or PSN ticket.
            if (!responseType.Equals(
                    "code",
                    StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode =
                    StatusCodes.Status400BadRequest;

                await context.Response.WriteAsync(
                    "Unsupported response_type.");

                return;
            }

            string separator =
                redirectUri.Contains('?') ? "&" : "?";

            string location =
                $"{redirectUri}{separator}" +
                "code=LOCAL_FIFA19_CODE";

            context.Response.StatusCode =
                StatusCodes.Status302Found;

            context.Response.Headers.Location =
                location;

            context.Response.Headers.CacheControl =
                "no-store";

            context.Response.Headers.Pragma =
                "no-cache";

            await context.Response.CompleteAsync();
        });

        app.MapGet(
            "/" +
            Program.ZamboniConfig.ApiServerIdentifier +
            "/status",
            () => Results.Json(new
            {
                serverVersion =
                    Program.ZamboniConfig
                        .ApiServerIdentifier,

                onlineUsersCount =
                    ServerManager
                        .GetServerPlayers()
                        .Count,

                onlineUsers =
                    string.Join(
                        ", ",
                        ServerManager
                            .GetServerPlayers()
                            .Values
                            .Select(
                                serverPlayer =>
                                    serverPlayer
                                        .UserIdentification
                                        .mName)),

                queuedUsers =
                    ServerManager
                        .GetQueuedPlayers()
                        .Count,

                activeGames =
                    ServerManager
                        .GetServerGames()
                        .Count
            }));

        // Helpful fallback so unknown FIFA HTTP paths are visible.
        app.MapFallback(async context =>
        {
            Console.WriteLine(
                $"Unhandled HTTP route: " +
                $"{context.Request.Method} " +
                $"{context.Request.Path}" +
                $"{context.Request.QueryString}");

            context.Response.StatusCode =
                StatusCodes.Status404NotFound;

            context.Response.ContentType =
                "text/plain";

            await context.Response.WriteAsync(
                "Not Found");
        });

        Console.WriteLine(
            $"FIFA local API listening on {_address}");

        await app.RunAsync(_address);
    }
}