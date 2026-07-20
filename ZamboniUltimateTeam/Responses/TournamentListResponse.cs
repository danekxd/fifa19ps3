using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct TournamentListResponse
{
    [TdfMember("TNOW")] 
    public uint mServerTime;
    
    [TdfMember("TRNS")] 
    public List<TournamentInfo> mTournaments;
    
}