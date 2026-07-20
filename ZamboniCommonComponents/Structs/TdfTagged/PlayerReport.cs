using Tdf;

namespace ZamboniCommonComponents.Structs.TdfTagged
{
    [TdfStruct]
    public struct PlayerReport
    {

        [TdfMember("CDNF")]
        public int mCDNF;
        
        [TdfMember("CHT")]
        public short mCHT;
        
        [TdfMember("CPRT")]
        public object? mCustomPlayerReport;
        
        [TdfMember("CSCO")]
        public int mCSCO;
        
        [TdfMember("CTRY")]
        public short mCTRY;
        
        [TdfMember("DISC")]
        public short mDISC;
        
        [TdfMember("FHRN")]
        public int mFHRN;
        
        [TdfMember("GRLT")]
        public int mGRLT;
        
        [TdfMember("GTAG")]
        public string mGTAG;
        
        [TdfMember("HOME")]
        public bool mHOME;
        
        [TdfMember("LOSS")]
        public int mLOSS;
        
        [TdfMember("NAME")]
        public string mNAME;
        
        [TdfMember("OPCT")]
        public int mOPCT;
        
        [TdfMember("OPPR")]
        public PrivatePlayerReport mPrivatePlayerReport;
        
        [TdfMember("OTL")]
        public int mOTL;
        
        [TdfMember("PEID")]
        public long mPEID;
        
        [TdfMember("PNID")]
        public long mPNID;
        
        [TdfMember("PPNA")]
        public string mPPNA;
        
        [TdfMember("PTAG")]
        public int mPTAG;
        
        [TdfMember("QUIT")]
        public short mQUIT;
        
        [TdfMember("RELT")]
        public int mRELT;
        
        [TdfMember("SCOR")]
        public int mSCOR;
        
        [TdfMember("SERG")]
        public short mSERG;
        
        [TdfMember("SKIL")]
        public int mSKIL;
        
        [TdfMember("SKPT")]
        public int mSKPT;
        
        [TdfMember("TEAM")]
        public int mTEAM;
        
        [TdfMember("TIES")]
        public int mTIES;
        
        [TdfMember("TNAM")]
        public string mTNAM;
        
        [TdfMember("WDNF")]
        public int mWDNF;
        
        [TdfMember("WGHT")]
        public int mWGHT;
        
        [TdfMember("WINS")]
        public int mWINS;

    }
}