using Tdf;

namespace Blaze3SDK.Blaze.Authentication
{
	[TdfStruct]
	public struct GetLegalDocContentRequest
	{

		[TdfMember("CPFT")]
		public CPFT mCPFT;
		
		[TdfMember("CTRY")]
		public string mIsoCountryCode;
		
		[TdfMember("FTCH")]
		public bool mFTCH;
		
		[TdfMember("LANG")]
		public string mIsoLanguage;
		
		[TdfMember("TEXT")]
		public ContentType mContentType;

		public enum ContentType : int
		{
			PLAIN = 0,
			HTML = 1,
		}
		public enum CPFT : int
		{
			ZERO = 0,
			ONE = 1,
			TWO = 2,
			THREE = 3,
			FOUR = 4,
			FIVE = 5,
			SIX = 6,
			SEVEN = 7,
			EIGHT = 8,
			NINE = 9,
			TEN = 10
		}

	}
}
