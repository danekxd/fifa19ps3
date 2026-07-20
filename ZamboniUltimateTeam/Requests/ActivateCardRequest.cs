using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ActivateCardRequest
{
    [TdfMember("ATYP")] 
    public CardState mActiveState;
    
    [TdfMember("CID")] 
    public long mCardId;
    
    [TdfMember("UID")] 
    public long mUserId;

}