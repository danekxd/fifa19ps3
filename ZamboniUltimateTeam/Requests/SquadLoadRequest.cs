using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct SquadLoadRequest
{
    
    [TdfMember("SQID")] 
    public int mSquadId;
    
    [TdfMember("UID")] 
    public long mUserId;
    
}