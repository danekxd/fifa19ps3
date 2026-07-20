using Tdf;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents.Responses
{
    [TdfStruct]
    public struct FetchGatesResponse
    {
        [TdfMember("LIST")] 
        public List<FeatureGate> mFeatureGatesList;
        
    }
}