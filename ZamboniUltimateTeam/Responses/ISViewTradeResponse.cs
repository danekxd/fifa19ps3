using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct ISViewTradeResponse
{
    
    [TdfMember("CRED")] 
    public int mCredits;
    
    [TdfMember("INFO")] 
    public ISTradeInfo mISTradeInfo;
    
}