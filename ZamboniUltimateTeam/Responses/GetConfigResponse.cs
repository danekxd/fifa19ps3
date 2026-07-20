using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct GetConfigResponse
{
    
    [TdfMember("GCFL")] 
    public List<int> mConfigList;
    
}