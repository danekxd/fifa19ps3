using Tdf;

namespace ZamboniUltimateTeam.Structs;

[TdfStruct]
public struct FriendGameEntry
{
    [TdfMember("OID")] 
    public byte mOpponentsGoals;

    [TdfMember("RSLT")] 
    public CardHouseGameResult mResult;
    
    [TdfMember("TIME")] 
    public uint mPlayedAt;
    
    [TdfMember("UID")] 
    public byte mMyGoals;
}