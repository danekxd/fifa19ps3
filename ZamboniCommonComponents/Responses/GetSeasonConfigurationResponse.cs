using Tdf;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents.Responses;

[TdfStruct]
public struct GetSeasonConfigurationResponse
{
    [TdfMember("CFGL")] 
    public List<SeasonConfiguration> mInstanceConfigList;

}