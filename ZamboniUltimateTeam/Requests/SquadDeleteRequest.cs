using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct SquadDeleteRequest
{
    
    [TdfMember("SQID")] 
    public int mSquadId;
    
    [TdfMember("UID")] 
    public long mUserId;
    
}