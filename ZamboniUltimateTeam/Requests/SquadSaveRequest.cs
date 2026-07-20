using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct SquadSaveRequest
{
    
    [TdfMember("CHEM")] 
    public uint mChemistry;
    
    [TdfMember("COPY")] 
    public byte mCopyCurrent;
    
    [TdfMember("FORM")] 
    public uint mFormation;
    
    [TdfMember("LINE")] 
    public List<int> mLines;
    
    [TdfMember("MNGR")] 
    public long mManager;
    
    [TdfMember("NAME")] 
    public string mSquadName;
    
    [TdfMember("PLRS")] 
    public List<long> mPlayers;
    
    [TdfMember("RDEF")] 
    public byte mRatingDefensive;
    
    [TdfMember("RGK")] 
    public byte mRatingGoalies;
    
    [TdfMember("ROFF")] 
    public byte mRatingOffensive;
    
    [TdfMember("RTNG")] 
    public uint mStarRating;
    
    [TdfMember("SQID")] 
    public int mSquadId;
    
    [TdfMember("UID")] 
    public long mUserId;
}