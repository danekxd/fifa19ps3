using Tdf;

namespace Blaze3SDK.Blaze.GameManager
{
	[TdfStruct]
	public struct NotifyMatchmakingFailed
	{

		[TdfMember("RSLT")]
		public MatchmakingResult mMatchmakingResult;

		[TdfMember("MAXF")]
		public uint mMaxPossibleFitScore;

		[TdfMember("MSID")]
		public ulong mSessionId;

		[TdfMember("USID")]
		public ulong mUserSessionId;

	}
}
