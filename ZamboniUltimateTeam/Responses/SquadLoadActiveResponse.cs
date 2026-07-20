using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct SquadLoadActiveResponse
{
    [TdfMember("ACTV")] 
    public List<CardData> mActiveCards;
    
    [TdfMember("SQAD")] 
    public SquadInfo mSquadInfo;
    
    [TdfMember("TUID")] 
    public long mTargetUserId;

}