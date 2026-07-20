using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct SquadSearchRequest
{
    [TdfMember("CLNT")] 
    public UltimateTeamClientType mClientType;

    [TdfMember("MAXN")] 
    public byte mMaxNumber;
    
    [TdfMember("NAME")] 
    public string mName;
    
    [TdfMember("TOR")] 
    public byte mTeamOverall;
    
    [TdfMember("UID")] 
    public long mUserId;
}