using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct SettingView
{
    [TdfMember("ID")] 
    public string mID;

    [TdfMember("LVDS")] 
    public List<SettingViewData> mSettingViewDataList;
    
}