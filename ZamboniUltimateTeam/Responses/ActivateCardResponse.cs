using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct ActivateCardResponse
{
    
    [TdfMember("CID")] 
    public long mCardId;
    
}