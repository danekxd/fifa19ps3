using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct StoreGetPackTypesRequest
{
    [TdfMember("GPID")] 
    public int mGroupId;

    [TdfMember("UID")] 
    public ulong mUserId;
}