using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct FeatureGate
{
    [TdfMember("BITF")] 
    public int mBITF;

    [TdfMember("ENDD")] 
    public long mENDD;
    
    [TdfMember("FEID")] 
    public string mFEID;
    
    [TdfMember("GRNT")] 
    public GrantMethod mGrantMethod;
    
    [TdfMember("STRT")] 
    public long mSTRT;
}