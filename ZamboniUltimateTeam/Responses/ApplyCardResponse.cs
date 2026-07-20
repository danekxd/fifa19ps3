using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct ApplyCardResponse
{
    
    [TdfMember("CDAT")] 
    public List<CardData> mCardDataList;
    
    [TdfMember("CID")] 
    public long mCardId;
    
    [TdfMember("UID")] 
    public ulong mUserId;
    
}