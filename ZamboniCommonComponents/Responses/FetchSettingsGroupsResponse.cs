using Tdf;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents.Responses;

[TdfStruct]
public struct FetchSettingsGroupsResponse
{
    
    [TdfMember("LGRP")] 
    public List<SettingGroup> mSettingGroupList;

}