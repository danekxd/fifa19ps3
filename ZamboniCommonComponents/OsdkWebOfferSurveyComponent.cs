using BlazeCommon;
using ZamboniCommonComponents.Bases;
using ZamboniCommonComponents.Requests;
using ZamboniCommonComponents.Responses;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents;

public class OsdkWebOfferSurveyComponent : OsdkWebOfferSurveyComponentBase.Server
{
    public override Task<GetSurveyListResponse> GetSurveyListAsync(GetSurveyListArgs request, BlazeRpcContext context)
    {
        return Task.FromResult(new GetSurveyListResponse
        {
            mSurveyList = new List<Survey>
            {
                new Survey
                {
                    mDescription = "SurveyDescription",
                    mSurveyIdOrScriptId = 1,
                    mTitle = "SurveyTitle"
                }
            }
        });
    }
}