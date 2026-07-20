using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct ISGetOffersResponse
{
    
    [TdfMember("LIST")] 
    public List<ISOfferInfo> mOfferList;
    
    [TdfMember("TOTC")] 
    public int mTotalCount;
    
}