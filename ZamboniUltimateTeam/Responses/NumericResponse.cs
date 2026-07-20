using Tdf;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct NumericResponse
{
    
    [TdfMember("NUM")] 
    public uint mNumber;
    
}