using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct Division
{
    [TdfMember("NUM")] 
    public uint mNumber;

    [TdfMember("SIZE")] 
    public byte mSize;
    
    [TdfMember("TRUL")] 
    public TournamentRule mTournamentRule;
}