using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct SquadRenameRequest
{
    [TdfMember("NAME")] 
    public string mNewName;

    [TdfMember("SQID")] 
    public int mSquadId;
    
    [TdfMember("UID")] 
    public ulong mUserId;
}