using Tdf;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents.Requests;

[TdfStruct]
public struct UpdateFiltersRequest
{
    [TdfMember("FILT")] 
    public TickerFilter mTickerFilter;

    [TdfMember("IDEN")] 
    public ulong mBlazeId;
}