using Tdf;

namespace ZamboniUltimateTeam.Structs;

[TdfStruct]
public struct StickerBookStatTotals
{
    [TdfMember("RWRD")] 
    public int mRWRD;
    
    [TdfMember("TCTJ")] 
    public int mTCTJ;
    
    [TdfMember("TCTL")] 
    public int mTCTL;
    
    [TdfMember("TCTP")] 
    public int mTCTP;
    
    [TdfMember("TEAM")] 
    public int mTEAM;
    
    [TdfMember("ULCK")] 
    public int mULCK;
    
}