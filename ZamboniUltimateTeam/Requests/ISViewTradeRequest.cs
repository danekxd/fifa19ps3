using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ISViewTradeRequest
{
    [TdfMember("REM")] 
    public int mRemove;

    [TdfMember("TID")] 
    public long mTradeId;
    
    [TdfMember("UID")] 
    public ulong mUserId;
    
}