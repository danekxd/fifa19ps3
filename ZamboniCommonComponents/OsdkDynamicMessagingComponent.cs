using BlazeCommon;
using ZamboniCommonComponents.Bases;
using ZamboniCommonComponents.Requests;
using ZamboniCommonComponents.Responses;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents;

public class OsdkDynamicMessagingComponent : OsdkDynamicMessagingComponentBase.Server
{
    public override Task<DynamicConfigResponse> GetConfigAsync(NullStruct request, BlazeRpcContext context)
    {
        return Task.FromResult(new DynamicConfigResponse
        {
            mDataRequestDelaySeconds = 100,
            mErrorRetryDelaySeconds = 100,
            mMessageDelayIntervalSeconds = 10,
            mMaximumMessageCount = 10
        });
    }

    public override Task<MessageResponse> GetMessagesAsync(MessageRequest request, BlazeRpcContext context)
    {
        return Task.FromResult(new MessageResponse
        {
            mDynamicMessageEnum = DynamicMessageEnum.DYNAMICMESSAGE_ENUM_SUCCESS,
            mMessagesList = new List<MessageItem>
            {
                new MessageItem
                {
                    mLinkData = "Sampletext A",
                    mFormat = DynamicMessageFormat.DYNAMICMESSAGE_FORMAT_PLAINTEXT,
                    mLinkHint = "Sampletext B",
                    mMessageId = 1,
                    mText = new List<MessagePart>
                    {
                        new MessagePart
                        {
                            mData = "HUT is now Live! \nZamboni.gg/discord",
                            mDuration = 100
                        }
                    },
                    mTitle = "Zamboni.gg",
                    mLinkType = DynamicMessageType.DYNAMICMESSAGE_TYPE_MARKETPLACE
                }
            }
        });
    }
}