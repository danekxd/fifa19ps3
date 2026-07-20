using Blaze3SDK.Blaze.Example;
using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct SquadSearchResponse
{
    [TdfMember("RTLT")] 
    public List<OfflineOpponentTeam> mResultList;

}