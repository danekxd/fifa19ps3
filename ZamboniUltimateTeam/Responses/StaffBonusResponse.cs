using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct StaffBonusResponse
{
    [TdfMember("SDAT")] 
    public StaffBonusInfo mStaffBonusInfo;

}