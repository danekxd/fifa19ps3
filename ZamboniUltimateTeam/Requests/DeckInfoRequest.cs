using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct DeckInfoRequest
{
    [TdfMember("PERS")] 
    public string mPersona;

    [TdfMember("UID")] 
    public ulong mUserId;
    
    [TdfMember("VER")] 
    public VersionInfo mVersionInfo;
    
}