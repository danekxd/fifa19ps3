using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ApplyCardRequest
{
    [TdfMember("CID")] 
    public long mCardId;
    
    [TdfMember("CIDT")] 
    public List<long> mTargetCards;
    
    [TdfMember("UID")] 
    public ulong mUserId;

}