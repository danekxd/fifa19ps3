using Tdf;

namespace ZamboniCommonComponents.Requests;

[TdfStruct]
public struct SeasonDetailsRequest
{
    [TdfMember("SID")] 
    public uint mSeasonId;

}