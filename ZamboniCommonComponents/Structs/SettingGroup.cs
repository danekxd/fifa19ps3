using Tdf;

namespace ZamboniCommonComponents.Structs;

[TdfStruct]
public struct SettingGroup
{
    [TdfMember("ID")] 
    public string mId;

    [TdfMember("LSET")] 
    public List<string> mSettingList;

    [TdfMember("LVWS")] 
    public List<SettingView> mViewList;
    
}