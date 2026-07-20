using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct ViewCardsResponse
{
    [TdfMember("CDAT")] 
    public List<CardData> mCardDataList;
    
}