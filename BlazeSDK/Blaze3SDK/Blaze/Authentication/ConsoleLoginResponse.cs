using Tdf;

namespace Blaze3SDK.Blaze.Authentication
{
    [TdfStruct]
    public struct ConsoleLoginResponse
    {

        [TdfMember("AGUP")]
        public bool mCanAgeUp;

        [TdfMember("ANON")]
        public bool mANON;
		
        [TdfMember("NTOS")]
        public bool mNeedsLegalDoc;
		
        [TdfMember("SESS")]
        public SessionInfo mSessionInfo;
		
        [TdfMember("SPAM")]
        public bool mIsOfLegalContactAge;
		
        [TdfMember("UNDR")]
        public bool mIsUnderAge;

    }
}