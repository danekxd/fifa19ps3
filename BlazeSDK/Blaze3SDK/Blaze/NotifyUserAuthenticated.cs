using Tdf;

namespace Blaze3SDK.Blaze
{
    [TdfStruct]
    public struct NotifyUserAuthenticated
    {

        [TdfMember("SUBS")]
        public bool mSUBS;

        [TdfMember("BUID")]
        public long mBlazeUserId;

    }
}