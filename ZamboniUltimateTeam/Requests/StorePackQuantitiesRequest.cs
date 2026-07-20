using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct StorePackQuantitiesRequest
{
    [TdfMember("PTIL")] 
    public List<short> mPackTypeIdList;

}