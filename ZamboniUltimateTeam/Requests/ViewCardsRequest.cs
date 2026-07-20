using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ViewCardsRequest
{
    [TdfMember("CARD")] 
    public List<long> mCardIdList;

    [TdfMember("UID")] 
    public ulong mUserId;
}