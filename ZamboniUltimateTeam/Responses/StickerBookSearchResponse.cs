using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct StickerBookSearchResponse
{
    [TdfMember("SRES")] 
    public List<CardData> mSearchResults;

}