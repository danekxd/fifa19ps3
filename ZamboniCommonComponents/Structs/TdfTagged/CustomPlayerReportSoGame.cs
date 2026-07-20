using Tdf;

namespace ZamboniCommonComponents.Structs.TdfTagged
{
    [TdfStruct(0x2810F02E)]
    public struct CustomPlayerReportSoGame
    {

        [TdfMember("GA")]
        public int mGA;
        
        [TdfMember("GF")]
        public int mGF;
        
        [TdfMember("SHTA")]
        public int mSHTA;
        
        [TdfMember("SHTS")]
        public int mSHTS;
        
        [TdfMember("SKLV")]
        public int mSKLV;
        
    }
}