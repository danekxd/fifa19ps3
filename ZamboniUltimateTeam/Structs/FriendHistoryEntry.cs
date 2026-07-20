using Tdf;

namespace ZamboniUltimateTeam.Structs;

[TdfStruct]
public struct FriendHistoryEntry
{
    [TdfMember("LOSS")] 
    public short mLosses;

    [TdfMember("OID")] 
    public long mOpponentId;
    
    [TdfMember("ONAM")] 
    public string mOpponentName;
    
    [TdfMember("OTL")] 
    public short mOverTimeLosses;
    
    [TdfMember("WINS")] 
    public short mWins;
}