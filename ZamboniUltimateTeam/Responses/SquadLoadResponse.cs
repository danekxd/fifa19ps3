using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct SquadLoadResponse
{
    [TdfMember("CHEM")] 
    public int mChemistry;

    [TdfMember("CHNG")] 
    public int mCHNG;
    
    [TdfMember("FORM")] 
    public int mFormation;
    
    [TdfMember("JERA")] 
    public uint mJerseyAwayDbId;
    
    [TdfMember("JERH")] 
    public uint mJerseyHomeDbId;
    
    [TdfMember("LINE")] 
    public List<int> mLines;
    
    [TdfMember("LOGO")] 
    public uint mLogoCardDbId;
    
    [TdfMember("MNGR")] 
    public CardData mManager;
    
    [TdfMember("NAME")] 
    public string mTeamName;
    
    [TdfMember("PLRS")] 
    public List<CardData> mPlayers;
    
    [TdfMember("RTNG")] 
    public int mStarRating;
    
    [TdfMember("SQID")] 
    public int mSquadId;
    
    [TdfMember("STAD")] 
    public uint mStadiumDbId;
    
    [TdfMember("TMAB")] 
    public string mTeamAbbreviation;
    
    [TdfMember("UID")] 
    public long mUserId;
    
}