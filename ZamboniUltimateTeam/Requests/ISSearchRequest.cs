using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ISSearchRequest
{
    [TdfMember("CAT")] 
    public CardSubTypeSearchParameter mCategory;

    [TdfMember("CTYP")] 
    public CardSearchTypeParameter mCardType;
    
    [TdfMember("FORM")] 
    public int mFormation;
    
    [TdfMember("IRET")] 
    public byte mIncludeRetired;
    
    [TdfMember("LEAG")] 
    public int mLeagueId;
    
    [TdfMember("LEV")] 
    public int mLevel;

    [TdfMember("MACR")] 
    public int mMaxCredits;
    
    [TdfMember("MAXB")] 
    public int mMaxBuyPrice;
    
    [TdfMember("MICR")] 
    public int mMinCredits;
    
    [TdfMember("MINB")] 
    public int mMinBuyPrice;

    [TdfMember("MYTR")] 
    public int mMyTrades;
    
    [TdfMember("NUMR")] 
    public int mNumRetrieve;
    
    [TdfMember("NOAC")] 
    public int mNonActive;
    
    [TdfMember("PLRT")] 
    public SpecialPlayerTypeParameter mSpecialPlayerTypeParameter;
    
    [TdfMember("POS")] 
    public int mPosition;

    [TdfMember("STRT")] 
    public int mStart;
    
    [TdfMember("TEAM")] 
    public int mTeamId;
    
    [TdfMember("UID")] 
    public ulong mUserId;
    
    [TdfMember("ZONE")] 
    public int mFieldZone;
    
}