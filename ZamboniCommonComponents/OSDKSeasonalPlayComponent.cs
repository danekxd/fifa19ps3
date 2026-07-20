using Blaze3SDK;
using BlazeCommon;
using ZamboniCommonComponents.Bases;
using ZamboniCommonComponents.Requests;
using ZamboniCommonComponents.Responses;
using ZamboniCommonComponents.Structs;

namespace ZamboniCommonComponents;

public class OSDKSeasonalPlayComponent : OSDKSeasonalPlayComponentBase.Server
{
    public override Task<GetSeasonConfigurationResponse> SeasonConfigurationRequestAsync(NullStruct request, BlazeRpcContext context)
    {
        // throw new BlazeRpcException(Blaze3RpcError.OSDKSEASONALPLAY_ERR_CONFIGURATION_ERROR);

        return Task.FromResult(new GetSeasonConfigurationResponse
        {
            mInstanceConfigList = new List<SeasonConfiguration>()
            {
                {
                    new SeasonConfiguration
                    {
                        // mDivisionList = new List<Division>
                        // {
                        //     new Division
                        //     {
                        //         mNumber = 0,
                        //         mSize = 10,
                        //         mTournamentRule = TournamentRule.SEASONALPLAY_TOURNAMENTRULE_UNLIMITED
                        //     },
                        //     new Division
                        //     {
                        //         mNumber = 1,
                        //         mSize = 10,
                        //         mTournamentRule = TournamentRule.SEASONALPLAY_TOURNAMENTRULE_UNLIMITED
                        //     },
                        //     new Division
                        //     {
                        //         mNumber = 3,
                        //         mSize = 20,
                        //         mTournamentRule = TournamentRule.SEASONALPLAY_TOURNAMENTRULE_UNLIMITED
                        //     },
                        // },


                        mLeagueID = 0,
                        mLeagueName = "",
                        mMemberType = MemberType.SEASONALPLAY_MEMBERTYPE_USER,
                        mSeasonID = 0,
                        mStatPeriodEnum = StatPeriod.STAT_PERIOD_ALLTIME,
                        mTeamID = 0
                    }
                },
            }
        });
    }

    public static uint TimeNow()
    {
        return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public override Task<SeasonDetails> SeasonDetailsRequestAsync(SeasonDetailsRequest request, BlazeRpcContext context)
    {
        // throw new BlazeRpcException(Blaze3RpcError.OSDKSEASONALPLAY_ERR_SEASON_NOT_FOUND);
        return Task.FromResult(new SeasonDetails
        {
            mNextRegularSeasonStart = TimeNow() + 10000,
            mPlayOffEnd = TimeNow() + 8000,
            mPlayOffStart = TimeNow() + 5000,
            mRegularSeasonEnd = TimeNow() + 4500,
            mRegularSeasonStart = TimeNow() - 1000,
            mSeasonId = request.mSeasonId,
            mSeasonNumber = request.mSeasonId,
            mSeasonState = SeasonState.SEASONALPLAY_SEASON_STATE_REGULARSEASON
        });
    }
}