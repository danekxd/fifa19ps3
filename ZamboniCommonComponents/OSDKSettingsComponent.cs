using BlazeCommon;
using ZamboniCommonComponents.Bases;
using ZamboniCommonComponents.Responses;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents;

public class OSDKSettingsComponent : OSDKSettingsComponentBase.Server
{
    public override Task<FetchSettingsResponse> FetchSettingsAsync(NullStruct request, BlazeRpcContext context)
    {
        return Task.FromResult(new FetchSettingsResponse
        {
            mStringSettingList = new List<SettingString>
            {
                new SettingString
                {
                    mId = "O_TKfilter",
                }
            }
        });
    }

    public override Task<FetchSettingsGroupsResponse> FetchSettingsGroupsAsync(NullStruct request, BlazeRpcContext context)
    {
        return Task.FromResult(new FetchSettingsGroupsResponse
        {
            mSettingGroupList = new List<SettingGroup>
            {
                new SettingGroup
                {
                    mId = "O_SG_TCKR",
                    mSettingList = new List<string>
                    {
                        "O_TKfilter"
                    }
                }
            }
        });
    }
}