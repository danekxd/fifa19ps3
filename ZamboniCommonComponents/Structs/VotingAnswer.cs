using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct VotingAnswer
{
    [TdfMember("ANID")] 
    public int mAnswerId;

    [TdfMember("MYAN")] 
    public int mMyAnswer;
    
    [TdfMember("TEXT")] 
    public string mText;
    
    [TdfMember("WGHT")] 
    public int mWeight;
    
}