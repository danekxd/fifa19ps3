using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct DiscardCardResponse
{
    [TdfMember("CRED")] 
    public int mCredits;

    [TdfMember("VER")] 
    public VersionInfo mVersionInfo;
    
}