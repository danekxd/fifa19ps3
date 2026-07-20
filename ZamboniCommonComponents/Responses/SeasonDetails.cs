using Tdf;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents.Responses;

[TdfStruct]
public struct SeasonDetails
{
    [TdfMember("NRST")] 
    public long mNextRegularSeasonStart;

    [TdfMember("PET")] 
    public long mPlayOffEnd;
    
    [TdfMember("PST")] 
    public long mPlayOffStart;

    [TdfMember("RET")] 
    public long mRegularSeasonEnd;
    
    [TdfMember("RST")] 
    public long mRegularSeasonStart;

    [TdfMember("SID")] 
    public uint mSeasonId;
    
    [TdfMember("SNUM")] 
    public uint mSeasonNumber;

    [TdfMember("STAT")] 
    public SeasonState mSeasonState;
}