using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct ISWatchListResponse
{
    
    [TdfMember("SRES")] 
    public List<ISTradeInfo> mTradeResults;
    
    [TdfMember("TOTC")] 
    public int mTotalCount;
    
}