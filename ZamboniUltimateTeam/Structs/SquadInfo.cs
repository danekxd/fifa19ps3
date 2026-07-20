using Tdf;

namespace ZamboniUltimateTeam.Structs;

[TdfStruct]
public struct SquadInfo
{
    [TdfMember("CHEM")] 
    public uint mChemistry;
    
    [TdfMember("CHNG")] 
    public uint mCHNG;
    
    [TdfMember("FORM")] 
    public uint mFormationId;
    
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
    public string mSquadName;

    [TdfMember("PLRS")] 
    public List<CardData> mPlayers;
    
    [TdfMember("RTNG")] 
    public uint mStarRating;
    
    [TdfMember("SQID")] 
    public int mSquadId;
    
    [TdfMember("STAD")] 
    public uint mStadiumDbId;
    
    [TdfMember("TMAB")] 
    public string mTeamAbbreviation;
    
}