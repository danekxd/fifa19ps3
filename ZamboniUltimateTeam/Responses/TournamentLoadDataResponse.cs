using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct TournamentLoadDataResponse
{
    
    [TdfMember("DATA")] 
    public byte[] mData;
    
}