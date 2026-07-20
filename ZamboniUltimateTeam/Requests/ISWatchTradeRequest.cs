using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ISWatchTradeRequest
{
    [TdfMember("TID")] 
    public long mTradeId;

    [TdfMember("UID")] 
    public long mUserId;
    
}