using Tdf;

namespace ZamboniCommonComponents.Requests;

[TdfStruct]
public struct GetMessagesRequest
{
    [TdfMember("IDEN")] 
    public ulong mBlazeId;

}