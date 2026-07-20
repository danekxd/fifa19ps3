using Tdf;

namespace ZamboniUltimateTeam.Structs;

[TdfStruct]
public struct VersionInfo
{
    [TdfMember("VESC")] 
    public uint mVersionEscrow;

    [TdfMember("VGEN")] 
    public uint mVersionGeneral;
    
    [TdfMember("VUNA")] 
    public uint mVersionUnassigned;
}