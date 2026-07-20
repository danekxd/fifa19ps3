using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct ISOfferTradeResponse
{
    
    [TdfMember("OID")] 
    public long mOfferId;
    
}