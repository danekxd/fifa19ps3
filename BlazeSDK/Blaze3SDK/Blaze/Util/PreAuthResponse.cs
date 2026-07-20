using Tdf;

namespace Blaze3SDK.Blaze.Util
{
    [TdfStruct]
    public struct PreAuthResponse
    {
		
        [TdfMember("ASRC")]
        public string mAuthenticationSource;

        [TdfMember("CIDS")]
        public List<ushort> mComponentIds;

        [TdfMember("CONF")]
        public FetchConfigResponse mConfig;

        [TdfMember("EEFA")]
        public bool mEEFA;

        [TdfMember("ESRC")]
        public string mESRC;
		
        [TdfMember("INST")]
        public string mINST;
		
        [TdfMember("MINR")]
        public bool mUnderageSupported;
		
        [TdfMember("NASP")]
        public string mPersonaNamespace;
		
        [TdfMember("PILD")]
        public string mLegalDocGameIdentifier;

        [TdfMember("PLAT")]
        public string mPlatform;

        [TdfMember("QOSS")]
        public QosConfigInfo mQosSettings;

        [TdfMember("RSRC")]
        public string mRegistrationSource;

        [TdfMember("SVER")]
        public string mServerVersion;

    }
}