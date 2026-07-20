using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct AssignCardsRequest
{
    [TdfMember("LIST")] 
    public List<AssignCardCard> mList;
    
    [TdfMember("UID")] 
    public long mUserId;

}