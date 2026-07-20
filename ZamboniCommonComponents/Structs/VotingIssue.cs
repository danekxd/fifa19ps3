using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct VotingIssue
{
    [TdfMember("ANSW")] 
    public List<VotingAnswer> mANSW;

    [TdfMember("CHVT")] 
    public byte mCHVT;
    
    [TdfMember("COUN")] 
    public int mCount;

    [TdfMember("ENDS")] 
    public uint mEnds;
    
    [TdfMember("ISSU")] 
    public uint mIssued;

    [TdfMember("MYAN")] 
    public uint mMyAnswer;
    
    [TdfMember("STRT")] 
    public uint mStart;

    [TdfMember("TEXT")] 
    public string mText;
    
    [TdfMember("WGHT")] 
    public uint mWeight;
    
}