using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ISAdminOfferRequest
{
    [TdfMember("OID")] 
    public long mOfferId;
    
    [TdfMember("STAT")] 
    public OfferState mOfferState;
    
    [TdfMember("UID")] 
    public ulong mUserId;

}