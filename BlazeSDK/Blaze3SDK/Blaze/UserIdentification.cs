using Tdf;

namespace Blaze3SDK.Blaze
{
    [TdfStruct]
    public struct UserIdentification
    {

        [TdfMember("AID")]
        public long mAccountId;

        [TdfMember("ALOC")]
        public uint mAccountLocale;

        [TdfMember("EXBB")]
        public byte[] mExternalBlob;
		
        [TdfMember("EXID")]
        public ulong mExternalId;
		
        [TdfMember("ID")]
        public long mBlazeId;

        [TdfMember("NAME")]
        public string mName;

        [TdfMember("ORIG")]
        public ulong mORIG;

    }
}