using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct NumericRequest
{
    
    [TdfMember("UID")] 
    public long mUserId;
    
}