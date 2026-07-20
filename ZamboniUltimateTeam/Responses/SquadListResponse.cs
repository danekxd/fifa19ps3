using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct SquadListResponse
{
    [TdfMember("ACTV")] 
    public int mActiveSquad;
    
    [TdfMember("SQDS")] 
    public List<SquadSmall> mSquads;

}