using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct SetSpecialRewardRequest
{
    
    [TdfMember("PARM")] 
    public int mPARM;
    
    [TdfMember("RWRD")] 
    public RewardFlag mRewardFlag;
    
    [TdfMember("UID")] 
    public long mUserId;
    
}