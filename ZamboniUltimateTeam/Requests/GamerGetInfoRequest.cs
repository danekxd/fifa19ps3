using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct GamerGetInfoRequest
{
    [TdfMember("CLNT")] 
    public UltimateTeamClientType mClientType;
    
    [TdfMember("TUID")] 
    public ulong mTargetUserId;

    [TdfMember("UID")] 
    public ulong mUserId;
    
}