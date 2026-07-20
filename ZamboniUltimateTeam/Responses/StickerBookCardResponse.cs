using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct StickerBookCardResponse
{
    [TdfMember("CRED")] 
    public int mTotalCredits;
    
    [TdfMember("VER")] 
    public VersionInfo mVersionInfo;

}