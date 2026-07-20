using BlazeCommon;
using ZamboniCommonComponents.Bases;
using ZamboniCommonComponents.Requests;
using ZamboniCommonComponents.Responses;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents;

public class OsdkTicker2Component : OsdkTicker2ComponentBase.Server
{
    public override Task<RegisterResponse> RegisterArgsAsync(RegisterArgs request, BlazeRpcContext context)
    {
        return Task.FromResult(new RegisterResponse()
        {
            mNumMessages = 2
        });
    }

    public override Task<GetMessagesResponse> GetMessagesAsync(GetMessagesRequest request, BlazeRpcContext context)
    {
        return Task.FromResult(new GetMessagesResponse()
        {
            mData = new List<TickerMessage>
            {
                new TickerMessage
                {
                    mData = new List<string>
                    {
                        "Join Zamboni.gg/discord"
                    },
                    mENDT = 10,
                    mFilterIndex = 10,
                    mBlazeId = 10,
                    mPRIO = 10,
                    mPROV = "Kaap0",
                    mSTRT = 10,
                    mType = TickerMessageType.TYPE_NEWS
                },
                new TickerMessage
                {
                    mData = new List<string>
                    {
                        "NHL 12 Hockey Ultimate Team is LIVE!"
                    },
                    mENDT = 10,
                    mFilterIndex = 10,
                    mBlazeId = 10,
                    mPRIO = 10,
                    mPROV = "Kaap0",
                    mSTRT = 10,
                    mType = TickerMessageType.TYPE_NEWS
                }
            }
        });
    }
    
    public override Task<TickerFilter> UpdateFiltersAsync(UpdateFiltersRequest request, BlazeRpcContext context)
    {
        return Task.FromResult(new TickerFilter()
        {
            mBottom = request.mTickerFilter.mBottom,
            mTop = request.mTickerFilter.mTop
        });
    }
}