using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct StickerBookCardRequest
{
    [TdfMember("CID")] 
    public long mCardId;

    [TdfMember("SWAP")] 
    public long mSwapCardId;
    
    [TdfMember("UID")] 
    public ulong mUserId;
}