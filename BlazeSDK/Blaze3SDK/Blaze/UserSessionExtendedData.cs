using Tdf;

namespace Blaze3SDK.Blaze
{
    [TdfStruct]
    public struct UserSessionExtendedData
    {

        [TdfMember("ADDR")]
        public NetworkAddress mAddress;

        [TdfMember("BPS")]
        public string mBestPingSiteAlias;

        [TdfMember("CMAP")]
        public SortedDictionary<uint, int> mClientAttributes;
		
        [TdfMember("CTY")]
        public string mCountry;
		
        [TdfMember("CVAR")]
        public object? mClientData;

        [TdfMember("DMAP")]
        public SortedDictionary<uint, long> mDataMap;
		
        [TdfMember("HWFG")]
        public HardwareFlags mHardwareFlags;
		
        [TdfMember("PSLM")]
        public List<int> mLatencyList;
		
        [TdfMember("QDAT")]
        public Util.NetworkQosData mQosData;

        [TdfMember("UATT")]
        public ulong mUserInfoAttribute;

        [TdfMember("ULST")]
        public List<BlazeObjectId> mBlazeObjectIdList;

    }
}