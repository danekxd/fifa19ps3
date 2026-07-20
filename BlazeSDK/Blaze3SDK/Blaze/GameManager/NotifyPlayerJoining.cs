using Tdf;

namespace Blaze3SDK.Blaze.GameManager
{
	[TdfStruct]
	public struct NotifyPlayerJoining
	{

		[TdfMember("GID")]
		public ulong mGameId;

		[TdfMember("PDAT")]
		public ReplicatedGamePlayer mJoiningPlayer;

	}
}
