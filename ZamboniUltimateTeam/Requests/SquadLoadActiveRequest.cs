using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct SquadLoadActiveRequest
{
    
    [TdfMember("TUID")] 
    public long mTargetUserId;
    
    [TdfMember("UID")] 
    public long mUserId;
    
}