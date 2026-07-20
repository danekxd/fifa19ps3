using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct GamerGetInfoResponse
{
    [TdfMember("INFO")] 
    public GamerInfo mGamerInfo;

    [TdfMember("UID")] 
    public ulong mUserId;
    
}