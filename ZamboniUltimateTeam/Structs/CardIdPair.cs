using Tdf;

namespace ZamboniUltimateTeam.Structs;

[TdfStruct]
public struct CardIdPair
{
    [TdfMember("CID")] 
    public long mCardId;

    [TdfMember("DCID")] 
    public long mDuplicateCardId;
}