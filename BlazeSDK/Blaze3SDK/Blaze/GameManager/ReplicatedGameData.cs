using Blaze3SDK.Blaze.Example;
using Blaze3SDK.Components;
using Tdf;

namespace Blaze3SDK.Blaze.GameManager
{
	[TdfStruct(0x3C1FCCF0)]
	public struct ReplicatedGameData
	{

		[TdfMember("ADMN")]
		public List<long> mAdminPlayerList;

		[TdfMember("ATTR")]
		public SortedDictionary<string, string> mGameAttribs;
		
		[TdfMember("CAP")]
		public List<ushort> mSlotCapacities;
		
		[TdfMember("CRIT")]
		public SortedDictionary<string, string> mEntryCriteriaMap;

		[TdfMember("GID")]
		public ulong mGameId;

		[TdfMember("GNAM")]
		public string mGameName;
		
		[TdfMember("GPVH")]
		public ulong mGameProtocolVersionHash;
		
		[TdfMember("GSET")]
		public GameSettings mGameSettings;
		
		[TdfMember("GSID")]
		public ulong mGameReportingId;
		
		[TdfMember("GSTA")]
		public GameState mGameState;
		
		[TdfMember("GTYP")]
		public string mGameTypeName;
		
		[TdfMember("GURL")]
		public string mGameStatusUrl;
		
		[TdfMember("HNET")]
		public List<NetworkAddress> mHostNetworkAddressList;
		
		[TdfMember("HSES")]
		public ulong mTopologyHostSessionId;
		
		[TdfMember("IGNO")]
		public bool mIgnoreEntryCriteriaWithInvite;
		
		[TdfMember("MATR")]
		public SortedDictionary<string, string> mMeshAttribs;
		
		[TdfMember("MCAP")]
		public ushort mMaxPlayerCapacity;
		
		[TdfMember("NQOS")]
		public Util.NetworkQosData mNetworkQosData;
		
		[TdfMember("NRES")]
		public bool mServerNotResetable;

		[TdfMember("NTOP")]
		public GameNetworkTopology mNetworkTopology;
		
		[TdfMember("PGID")]
		public string mPersistedGameId;

		[TdfMember("PGSR")]
		public byte[] mPersistedGameIdSecret;

		[TdfMember("PHST")]
		public HostInfo mPlatformHostInfo;
		
		[TdfMember("PRES")]
		public PresenceMode mPresenceMode;
		
		[TdfMember("PSAS")]
		public string mPingSiteAlias;

		[TdfMember("QCAP")]
		public ushort mQueueCapacity;
		
		[TdfMember("SEED")]
		public uint mSharedSeed;

		[TdfMember("TCAP")]
		public ushort mTeamCapacity;

		[TdfMember("THST")]
		public HostInfo mTopologyHostInfo;
		
		[TdfMember("TIDS")]
		public List<ushort> mTeamIds;
		
		[TdfMember("UUID")]
		public string mUUID;

		[TdfMember("VOIP")]
		public VoipTopology mVoipNetwork;

		[TdfMember("VSTR")]
		public string mGameProtocolVersionString;
		
		[TdfMember("XNNC")]
		public byte[] mXnetNonce;

		[TdfMember("XSES")]
		public byte[] mXnetSession;

	}
}
