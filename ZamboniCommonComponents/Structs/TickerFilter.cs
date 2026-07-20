using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct TickerFilter
{
    [TdfMember("BOT_")] 
    public long mBottom;

    [TdfMember("TOP_")] 
    public long mTop;
    
}