using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct TournamentLoadDataRequest
{
    
    [TdfMember("TYPE")] 
    public SaveTournamentType mTournamentType;
    
    [TdfMember("UID")] 
    public long mUserId;

}