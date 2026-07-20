using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct GetPersistenceInfoResponse
{
    
    [TdfMember("EST")] 
    public int mEST;
    
    [TdfMember("VLUE")] 
    public int mVLUE;
    
}