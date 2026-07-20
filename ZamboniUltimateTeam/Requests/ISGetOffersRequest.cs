using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ISGetOffersRequest
{
    [TdfMember("MSID")] 
    public uint mMSID;
    
    [TdfMember("NOAC")] 
    public uint mNonActive;
    
    [TdfMember("NUMR")] 
    public int mNumRetrieve;
    
    [TdfMember("STRT")] 
    public uint mStart;
    
    [TdfMember("TID")] 
    public long mTradeId;
    
    [TdfMember("UID")] 
    public long mUserId;

}