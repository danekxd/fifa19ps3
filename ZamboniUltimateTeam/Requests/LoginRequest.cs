using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct LoginRequest
{
    [TdfMember("CLNT")] 
    public UltimateTeamClientType mClientType;
    
    [TdfMember("CP")] 
    public uint mCreatePlayer;

    [TdfMember("PERS")] 
    public string mPersona;

    [TdfMember("PUR")] 
    public uint mPurchased;

    [TdfMember("UID")] 
    public ulong mUserId;
}