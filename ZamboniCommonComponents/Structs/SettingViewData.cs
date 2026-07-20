using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct SettingViewData
{
    [TdfMember("DEFS")] 
    public string mDefaultStr;

    [TdfMember("HLAB")] 
    public string mHelpLabel;

    [TdfMember("ID")] 
    public string mId;

    [TdfMember("TOGG")] 
    public uint mToggles;
    
    [TdfMember("VAL")] 
    public string mValueStr;
    
}