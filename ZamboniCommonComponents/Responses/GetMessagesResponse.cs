using Tdf;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents.Responses;

[TdfStruct]
public struct GetMessagesResponse
{
    
    [TdfMember("DATA")] 
    public List<TickerMessage> mData;
    
}