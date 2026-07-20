using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct SquadSaveResponse
{
    [TdfMember("SQID")] 
    public int mSquadId;

}