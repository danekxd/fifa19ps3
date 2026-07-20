using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct ApplySalaryCapResponse
{
    [TdfMember("PID")] 
    public long mPlayerCardId;
    
    [TdfMember("SAL")] 
    public short mSalaryCap;
    
    [TdfMember("UID")] 
    public long mUserId;

}