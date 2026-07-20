using BlazeCommon;
using ZamboniCommonComponents.Bases;
using ZamboniCommonComponents.Responses;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents;

public class OsdkOnlinePassComponent : OsdkOnlinePassComponentBase.Server
{
    public override Task<FetchGatesResponse> FetchGatesAsync(NullStruct request, BlazeRpcContext context)
    {
        return Task.FromResult(new FetchGatesResponse
        {
            mFeatureGatesList = new List<FeatureGate>()
        });
    }
}