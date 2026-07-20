using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct GetConfigRequest
{
    [TdfMember("CLNT")] 
    public UltimateTeamClientType mClientType;

    [TdfMember("UID")] 
    public long mUserId;
    
}