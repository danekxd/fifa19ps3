using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct AssignCardsResponse
{
    
    [TdfMember("VER")] 
    public VersionInfo mVersionInfo;
    
}