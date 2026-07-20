using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct VoteCastNotificationType
{
    [TdfMember("ANID")] 
    public int mAnswerId;

    [TdfMember("COUN")] 
    public int mCount;
    
    [TdfMember("ISSU")] 
    public uint mIssued;
    
}