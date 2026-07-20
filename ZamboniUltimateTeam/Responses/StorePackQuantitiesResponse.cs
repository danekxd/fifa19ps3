using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct StorePackQuantitiesResponse
{
    [TdfMember("PQTL")] 
    public List<int> mPackQuantitiesList;
    
}