using Tdf;

namespace ZamboniUltimateTeam.Structs;

[TdfStruct]
public struct ISOfferInfo
{
    [TdfMember("CARD")] 
    public List<long> mCardList;

    [TdfMember("CDAT")] 
    public List<CardData> mCardDataList;
    
    [TdfMember("CRED")] 
    public int mCredits;

    [TdfMember("OID")] 
    public long mOfferId;
    
    [TdfMember("STAT")] 
    public OfferState mOfferState;

    [TdfMember("TID")] 
    public long mTradeId;
    
    [TdfMember("UID")] 
    public long mUserId;

}