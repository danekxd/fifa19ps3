using Tdf;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents.Responses;

[TdfStruct]
public struct GetSurveyListResponse
{
    [TdfMember("LIST")] 
    public List<Survey> mSurveyList;

}