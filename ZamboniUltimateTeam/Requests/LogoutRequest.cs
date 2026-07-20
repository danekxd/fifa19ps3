using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct LogoutRequest
{
    [TdfMember("DU")] 
    public uint mDiscardUnassigned;

    [TdfMember("UID")] 
    public long mUserId;
    
}