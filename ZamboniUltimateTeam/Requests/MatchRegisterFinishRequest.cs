using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct MatchRegisterFinishRequest
{
    [TdfMember("ID")] 
    public long mId;

    [TdfMember("STAT")] 
    public MatchState mMatchState;

    [TdfMember("UID")] 
    public ulong mUserId;
}