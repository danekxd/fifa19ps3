using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct MessagePart
{
    [TdfMember("DATA")] 
    public string mData;

    [TdfMember("DURN")] 
    public int mDuration;
}