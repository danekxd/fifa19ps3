using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct StickerBookSearchRequest
{
    [TdfMember("BASE")] 
    public byte mBASE;
    
    [TdfMember("COLL")] 
    public byte mCollectionYearId;

    [TdfMember("COUN")] 
    public int mCountryId;
    
    [TdfMember("CTYP")] 
    public CollectionSearchType mCollectionSearchCardType;
    
    [TdfMember("FORM")] 
    public int mFormation;
    
    [TdfMember("LEAG")] 
    public int mLeagueId;
    
    [TdfMember("LEV")] 
    public CardLevel mCardLevel;
    
    [TdfMember("NAT")] 
    public int mNation;
    
    [TdfMember("NUMR")] 
    public int mNumRetrieve;
    
    [TdfMember("POS")] 
    public int mPosition;
    
    [TdfMember("STAT")] 
    public CardState mCardState;
    
    [TdfMember("STRT")] 
    public int mStart;
    
    [TdfMember("TEAM")] 
    public int mTeamId;
    
    [TdfMember("UID")] 
    public long mUserId;

}