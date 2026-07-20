using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct StoreGetPackTypesResponse
{
    [TdfMember("FRPK")] 
    public short mFreePack;
    
    [TdfMember("PPH")] 
    public byte mPremiumPacksHidden;
    
    [TdfMember("PTPS")] 
    public List<StorePackTypeData> mPackTypeList;
    
    [TdfMember("SVTM")] 
    public uint mServerTime;
    
}