using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ChangePlayersRequest
{
    [TdfMember("CARD")] 
    public List<CardData> mCardDataList;
    
    [TdfMember("UID")] 
    public long mUserId;

}