using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct TournamentSaveDataRequest
{
    [TdfMember("DATA")] 
    public byte[] mData;
    
    [TdfMember("TYPE")] 
    public SaveTournamentType mTournamentType;
    
    [TdfMember("UID")] 
    public long mUserId;

}