using Tdf;

namespace ZamboniCommonComponents.Requests;

[TdfStruct]
public struct MessageRequest
{
    [TdfMember("LCAL")] 
    public string mLocale;

    [TdfMember("MODE")] 
    public string mMode;

    [TdfMember("TITL")] 
    public string mTitle;

    [TdfMember("TOKN")] 
    public string mToken;
}