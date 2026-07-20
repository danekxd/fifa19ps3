using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct GetFriendGameListRequest
{
    [TdfMember("MAX")] 
    public short mMaxResults;
    
    [TdfMember("OID")] 
    public long mOpponentId;
    
    [TdfMember("UID")] 
    public long mUserId;

}