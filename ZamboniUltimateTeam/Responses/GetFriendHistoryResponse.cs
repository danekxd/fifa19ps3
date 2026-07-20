using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct GetFriendHistoryResponse
{
    
    [TdfMember("HIST")] 
    public List<FriendHistoryEntry> mHistoryList;
    
}