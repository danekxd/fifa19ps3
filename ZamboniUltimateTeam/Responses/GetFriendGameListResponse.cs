using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct GetFriendGameListResponse
{
    
    [TdfMember("LIST")] 
    public List<FriendGameEntry> mGameList;
    
}