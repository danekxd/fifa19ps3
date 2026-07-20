using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct StickerBookStats2Request
{
    [TdfMember("BASE")] 
    public byte mBASE;
    
    [TdfMember("CONT")] 
    public RequestContext mContextId;

    [TdfMember("UID")] 
    public long mUserId;
    
    [TdfMember("VALU")] 
    public int mValue;
    
    [TdfMember("YEAR")] 
    public byte mYearId;
    
}