using Tdf;

namespace ZamboniCommonComponents.Requests;

[TdfStruct]
public struct GetSurveyListArgs
{
    [TdfMember("DOLO")] 
    public int mDOLO;

    [TdfMember("LOC")] 
    public string mLocale;

    [TdfMember("USRF")] 
    public int mUSRF;

}