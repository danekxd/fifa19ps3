using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct ISSearchResponse
{
    
    [TdfMember("SRES")] 
    public List<ISTradeInfo> mSearchResults;
    
    [TdfMember("TOTC")] 
    public int mTotalCount;
    
}