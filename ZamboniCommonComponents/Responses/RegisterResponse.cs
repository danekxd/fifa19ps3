using Tdf;

namespace ZamboniCommonComponents.Responses;

[TdfStruct]
public struct RegisterResponse
{
    
    [TdfMember("MSGS")] 
    public uint mNumMessages;
    
}