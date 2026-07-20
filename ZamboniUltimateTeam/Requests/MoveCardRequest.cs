using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct MoveCardRequest
{
    
    [TdfMember("CID")] 
    public long mCardId;
    
    [TdfMember("DECK")] 
    public DeckType mDeckType;
    
    [TdfMember("SWAP")] 
    public long mSwapCardId;
    
    [TdfMember("UID")] 
    public ulong mUserId;
    
}