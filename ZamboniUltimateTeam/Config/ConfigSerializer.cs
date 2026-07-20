using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ZamboniUltimateTeam.Config;

public static class ConfigSerializer
{
    private static IDeserializer BuildDeserializer() =>
        new DeserializerBuilder()
            .WithNamingConvention(NullNamingConvention.Instance)
            .WithCaseInsensitivePropertyMatching()
            .IgnoreUnmatchedProperties()
            .Build();

    private static ISerializer BuildSerializer() =>
        new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .Build();

    public static T Deserialize<T>(string yaml) =>
        BuildDeserializer().Deserialize<T>(yaml);

    public static T DeserializeFile<T>(string path) =>
        Deserialize<T>(File.ReadAllText(path));

    public static string Serialize<T>(T config) =>
        BuildSerializer().Serialize(config);

    public static void SerializeToFile<T>(T config, string path) =>
        File.WriteAllText(path, Serialize(config));
}