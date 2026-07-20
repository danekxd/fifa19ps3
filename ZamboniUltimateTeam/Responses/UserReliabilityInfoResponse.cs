using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct UserReliabilityInfoResponse
{
    [TdfMember("DISC")] 
    public byte mPreviousMatchUnfinished;
    
    [TdfMember("MFI")] 
    public int mMatchesFinished;
    
    [TdfMember("MST")] 
    public int mMatchesStarted;
    
    [TdfMember("OID")] 
    public long mOID;
    
    [TdfMember("REL")] 
    public int mReliability;
    
    [TdfMember("UID")] 
    public long mUserId;

}