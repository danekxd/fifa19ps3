using Tdf;

namespace Blaze3SDK.Blaze.Authentication
{
    [TdfStruct]
    public struct PersonaDetails
    {
        //0x0223C3E0
        [TdfMember("DSNM")]
        public string mDisplayName;

        [TdfMember("LAST")]
        public uint mLastAuthenticated;
		
        [TdfMember("PID")]
        public long mPersonaId;
		
        [TdfMember("PLAT")]
        public ExternalSystemId mPlatform;
		
        [TdfMember("STAS")]
        public PersonaStatus mStatus;
		
        [TdfMember("XREF")]
        public ulong mExtId;
        
        [TdfMember("XTYP")]
        public ExternalSystemId mExternalSystemId;

    }
}