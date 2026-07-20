using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct MoveCardResponse
{
    [TdfMember("CID")] 
    public long mDisplacedCardId;

    [TdfMember("DECK")] 
    public DeckType mDisplacedDeckType;
    
    [TdfMember("POS")] 
    public uint mDisplacedCardPosition;
    
    [TdfMember("VER")] 
    public VersionInfo mVersionInfo;
}