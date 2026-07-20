using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ISWatchListRequest
{
    [TdfMember("NUM")] 
    public byte mPageSize;

    [TdfMember("ST")] 
    public short mStart;
    
    [TdfMember("UID")] 
    public ulong mUserId;
    
}