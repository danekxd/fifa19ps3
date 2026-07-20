using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ISOfferTradeRequest
{
    [TdfMember("CARD")] 
    public List<long> mCardList;
    
    [TdfMember("CRED")] 
    public int mCredits;
    
    [TdfMember("TID")] 
    public long mTradeId;
    
    [TdfMember("UID")] 
    public ulong mUserId;

}