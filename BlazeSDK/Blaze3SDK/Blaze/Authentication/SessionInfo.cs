using Tdf;

namespace Blaze3SDK.Blaze.Authentication
{
    [TdfStruct]
    public struct SessionInfo
    {

        [TdfMember("BUID")]
        public long mBlazeUserId;

        [TdfMember("FRST")]
        public bool mIsFirstLogin;
		
        [TdfMember("KEY")]
        public string mSessionKey;

        [TdfMember("LLOG")]
        public long mLastLoginDateTime;

        [TdfMember("MAIL")]
        public string mEmail;
		
        [TdfMember("PDTL")]
        public PersonaDetails mPersonaDetails;

        [TdfMember("UID")]
        public long mUserId;

    }
}