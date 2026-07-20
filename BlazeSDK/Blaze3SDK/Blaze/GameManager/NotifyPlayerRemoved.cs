using Tdf;

namespace Blaze3SDK.Blaze.GameManager
{
	[TdfStruct]
	public struct NotifyPlayerRemoved
	{

		[TdfMember("GID")]
		public ulong mGameId;

		[TdfMember("PID")]
		public long mPlayerId;

		[TdfMember("REAS")]
		public PlayerRemovedReason mPlayerRemovedReason;

		[TdfMember("CNTX")]
		public ushort mPlayerRemovedTitleContext;

	}
}
