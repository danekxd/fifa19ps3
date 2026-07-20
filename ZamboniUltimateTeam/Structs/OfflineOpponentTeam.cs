using Tdf;

namespace ZamboniUltimateTeam.Structs;

[TdfStruct]
public struct OfflineOpponentTeam
{
    [TdfMember("LOGO")] 
    public uint mLogoDbId;

    [TdfMember("OID")] 
    public long mOpponentId;
    
    [TdfMember("RDEF")] 
    public byte mRatingDefensive;

    [TdfMember("RGK")] 
    public byte mRatingGoalie;
    
    [TdfMember("ROFF")] 
    public byte mRatingOffensive;

    [TdfMember("RTNG")] 
    public byte mStarRating;
    
    [TdfMember("SQID")] 
    public int mSquadId;

    [TdfMember("TABR")] 
    public string mTeamAbbreviation;
    
    [TdfMember("TMNM")] 
    public string mTeamName;

    [TdfMember("TOPT")] 
    public byte mTOPT;
    
}