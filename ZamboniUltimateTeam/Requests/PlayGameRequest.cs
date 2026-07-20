using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct PlayGameRequest
{
    
    [TdfMember("ACID")] 
    public List<long> mGameCards;
    
    [TdfMember("CRED")] 
    public int mCredits;
    
    [TdfMember("GTIC")] 
    public int mGoldenTickets;
    
    [TdfMember("PGMR")] 
    public MatchResult mMatchResult;
    
    [TdfMember("PRES")] 
    public int mPrestige;
    
    [TdfMember("STAT")] 
    public PlayGameState mState;
    
    [TdfMember("TID")] 
    public int mTournamentId;
    
    [TdfMember("TTYP")] 
    public TournamentType mTournamentType;
    
    [TdfMember("UID")] 
    public ulong mUserId;
    
}