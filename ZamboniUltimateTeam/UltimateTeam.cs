using NLog;
using ZamboniUltimateTeam.Config;

namespace ZamboniUltimateTeam;

public static class UltimateTeam
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static IServerProvider Server;
    public static PackConfig PackConfig { get; private set; }
    public static TournamentConfig TournamentConfig { get; private set; }
    public static HutConfig HutConfig { get; private set; }
    private static string _packConfigPath;
    private static string _tournamentConfigPath;
    private static string _hutConfigConfigPath;


    public static void Initialize(string connectionString, IServerProvider provider)
    {
        UltimateDatabase.ConnectionString = connectionString;
        UltimateDatabase.CreateTables();
        Server = provider;

        _packConfigPath = "packs.yml";
        _tournamentConfigPath = "tournaments.yml";
        _hutConfigConfigPath = "hut-config.yml";
        ReloadConfigs();
    }

    public static void ReloadConfigs()
    {
        PackConfig = LoadOrCreate(_packConfigPath, ConfigSerializer.DeserializeFile<PackConfig>, ConfigSerializer.SerializeToFile);
        TournamentConfig = LoadOrCreate(_tournamentConfigPath, ConfigSerializer.DeserializeFile<TournamentConfig>, ConfigSerializer.SerializeToFile);
        HutConfig = LoadOrCreate(_hutConfigConfigPath, ConfigSerializer.DeserializeFile<HutConfig>, ConfigSerializer.SerializeToFile);
    }

    private static T LoadOrCreate<T>(string path, Func<string, T> deserialize, Action<T, string> serialize) where T : new()
    {
        if (!File.Exists(path))
        {
            var config = new T();
            serialize(config, path);
            return config;
        }

        try
        {
            return deserialize(path);
        }
        catch (Exception e)
        {
            Logger.Fatal("Configuration error in hut configs");
            Logger.Fatal(e);
            throw;
        }
    }

    public static uint TimeNowSeconds()
    {
        return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }
}