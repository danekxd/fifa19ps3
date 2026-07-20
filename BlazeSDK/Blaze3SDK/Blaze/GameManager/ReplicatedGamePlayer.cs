using Tdf;

namespace Blaze3SDK.Blaze.GameManager
{
	[TdfStruct(0xC86D3EA7)]
	public struct ReplicatedGamePlayer
	{

		[TdfMember("BLOB")]
		public byte[] mCustomData;
		
		[TdfMember("EXID")]
		public ulong mExternalId;
		
		[TdfMember("GID")]
		public ulong mGameId;
		
		[TdfMember("LOC")]
		public uint mAccountLocale;

		[TdfMember("NAME")]
		public string mPlayerName;

		[TdfMember("PATT")]
		public SortedDictionary<string, string> mPlayerAttribs;

		[TdfMember("PID")]
		public long mPlayerId;
		
		[TdfMember("PNET")]
		public NetworkAddress mNetworkAddress;

		[TdfMember("SID")]
		public byte mSlotId;

		[TdfMember("SLOT")]
		public SlotType mSlotType;

		[TdfMember("STAT")]
		public PlayerState mPlayerState;
		
		[TdfMember("TIDX")]
		public ushort mTeamIndex;
		
		[TdfMember("TIME")]
		public long mJoinedGameTimestamp;

		[TdfMember("UGID")]
		public BlazeObjectId mUserGroupId;

		[TdfMember("UID")]
		public ulong mPlayerSessionId;

	}
}
