namespace Zamboni14Legacy;

public class Api
{
    private readonly string _address;

    public Api()
    {
        _address = "http://0.0.0.0:"+Program.ZamboniConfig.ApiServerPort;
    }

    public async Task StartAsync()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        app.MapGet("/connect/auth", async context =>
        {
            string clientId =
                context.Request.Query["client_id"]
                    .FirstOrDefault() ?? "";

            string responseType =
                context.Request.Query["response_type"]
                    .FirstOrDefault() ?? "";

            string redirectUri =
                context.Request.Query["redirect_uri"]
                    .FirstOrDefault() ?? "nucleus:rest";

            Console.WriteLine(
                $"FIFA identity request: " +
                $"client_id={clientId}, " +
                $"response_type={responseType}, " +
                $"redirect_uri={redirectUri}, " +
                $"hasAuthorization=" +
                $"{context.Request.Headers.ContainsKey("Authorization")}"
            );

            // Do not print the Authorization header or PSN ticket.
            string separator =
                redirectUri.Contains('?') ? "&" : "?";

            string location =
                $"{redirectUri}{separator}" +
                $"code=LOCAL_FIFA19_CODE";

            context.Response.StatusCode =
                StatusCodes.Status302Found;

            context.Response.Headers.Location = location;

            await context.Response.CompleteAsync();
        });

        app.MapGet("/" + Program.ZamboniConfig.ApiServerIdentifier + "/status", () => Results.Json(new
        {
            serverVersion = Program.ZamboniConfig.ApiServerIdentifier,
            onlineUsersCount = ServerManager.GetServerPlayers().Count,
            onlineUsers = string.Join(", ", ServerManager.GetServerPlayers().Values.Select(serverPlayer => serverPlayer.UserIdentification.mName)),
            queuedUsers = ServerManager.GetQueuedPlayers().Count,
            activeGames = ServerManager.GetServerGames().Count
        }));

        await app.RunAsync(_address);
    }
}