using Tdf;

namespace Blaze3SDK.Blaze.GameManager
{
	[TdfStruct]
	public struct JoinGameResponse
	{

		[TdfMember("GID")]
		public ulong mGameId;

		[TdfMember("JGS")]
		public JoinState mJoinState;

	}
}
