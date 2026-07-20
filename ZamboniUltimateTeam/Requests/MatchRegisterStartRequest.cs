using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct MatchRegisterStartRequest
{
    [TdfMember("GMID")] 
    public uint mGameId;

    [TdfMember("ONL")] 
    public byte mOnlineGame;

    [TdfMember("TID")] 
    public uint mTournamentId;

    [TdfMember("UID")] 
    public ulong mUserId;
}