using Tdf;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents.Responses;

[TdfStruct]
public struct FetchSettingsResponse
{
    
    [TdfMember("LSIN")] 
    public List<SettingInteger> mIntegerSettingList;

    [TdfMember("LSST")] 
    public List<SettingString> mStringSettingList;
    
}