using YamlDotNet.Serialization;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Config
{
    public class Tournament
    {
        public int BlazeTournamentId { get; set; }
        public int TournamentId { get; set; }
        public int TrophyCardDbId { get; set; }
        public int Type { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int AiGroup { get; set; }
        public int Difficulty { get; set; }
        public int Elg1Type { get; set; }
        public int Elg1Data { get; set; }
        public int Elg2Type { get; set; }
        public int Elg2Data { get; set; }
        public int MatchLenght { get; set; }
        public int Prize { get; set; }
        public int Reward1 { get; set; }
        public int Reward2 { get; set; }
        public int Reward3 { get; set; }
        public int Reward4 { get; set; }
        public int SalaryCap { get; set; }
        public int TrophiesRequiredToEnter { get; set; }

        [YamlIgnore]
        public TournamentInfo TournamentInfo => new()
        {
            mAiGroup = (AiGroup)AiGroup,
            mBlazeTournamentId = BlazeTournamentId,
            mDifficulty = Difficulty,
            mElg1Type = (ElgType)Elg1Type,
            mElg1Data = Elg1Data,
            mElg2Type = (ElgType)Elg2Type,
            mElg2Data = Elg2Data,
            mEndTime = (uint)EndTime,
            mMatchLenght = MatchLenght,
            mPrize = Prize,
            mReward1 = Reward1,
            mReward2 = Reward2,
            mReward3 = Reward3,
            mReward4 = Reward4,
            mSalaryCap = SalaryCap,
            mStartTime = (uint)StartTime,
            mTrophyCardDbId = TrophyCardDbId,
            mTournamentId = TournamentId,
            mType = Type,
            mTrophiesRequiredToEnter = TrophiesRequiredToEnter
        };
    }
}