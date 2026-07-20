using Tdf;

namespace ZamboniCommonComponents.Structs.TdfTagged
{
    [TdfStruct]
    public struct GameInfoReport
    {

        [TdfMember("ARID")]
        public ulong mARID;
        
        [TdfMember("CGRT")]
        public object? mClubReport;
        
        [TdfMember("CTID")]
        public int mCTID;
        
        [TdfMember("GRID")]
        public ulong mGRID;
        
        [TdfMember("GTIM")]
        public int mGTIM;
        
        [TdfMember("ISIM")]
        public bool mISIM;
        
        [TdfMember("LGID")]
        public int mLGID;
        
        [TdfMember("NPER")]
        public short mNPER;
        
        [TdfMember("OVRT")]
        public int mOVRT;
        
        [TdfMember("PLEN")]
        public short mPLEN;
        
        [TdfMember("RANK")]
        public bool mRANK;
        
        [TdfMember("ROID")]
        public int mROID;
        
        [TdfMember("SEID")]
        public int mSEID;
        
        [TdfMember("SHOO")]
        public int mSHOO;
        
        [TdfMember("SKIL")]
        public short mSKIL;
        
        [TdfMember("SKU")]
        public short mSKU;
        
        [TdfMember("STUS")]
        public int mSTUS;
        
        [TdfMember("TYPE")]
        public string mTYPE;
        
        [TdfMember("VENU")]
        public short mVENU;

    }
}