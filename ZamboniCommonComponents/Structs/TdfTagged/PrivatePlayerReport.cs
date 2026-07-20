using Tdf;

namespace ZamboniCommonComponents.Structs.TdfTagged
{
    [TdfStruct]
    public struct PrivatePlayerReport
    {

        [TdfMember("PIAM")]
        public SortedDictionary<string, long> mPrivateIntAttributeMap;
        
        [TdfMember("PPAM")]
        public SortedDictionary<string, string> mPrivateAttributeMap;
        
    }
}