using Tdf;

namespace ZamboniUltimateTeam.Structs;

[TdfStruct]
public struct GeneralInfo
{
    [TdfMember("CRED")] 
    public int mCredits;

    [TdfMember("STAT")] 
    public List<int> mStats;
}