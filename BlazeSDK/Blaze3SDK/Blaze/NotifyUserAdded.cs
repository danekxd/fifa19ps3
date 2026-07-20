using Tdf;

namespace Blaze3SDK.Blaze
{
	[TdfStruct]
	public struct NotifyUserAdded
	{

		[TdfMember("USER")]
		public UserIdentification mUserInfo;

	}
}
