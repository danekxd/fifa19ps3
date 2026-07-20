using Tdf;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents.Responses;

[TdfStruct]
public struct MessageResponse
{
    [TdfMember("ENUM")] 
    public DynamicMessageEnum mDynamicMessageEnum;

    [TdfMember("MSGS")] 
    public List<MessageItem> mMessagesList;
}