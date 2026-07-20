using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct DiscardCardRequest
{
    [TdfMember("CID")] 
    public long mCardId;

    [TdfMember("CRED")] 
    public int mCredits;
    
    [TdfMember("UID")] 
    public long mUserId;
    
}