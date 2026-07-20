using System.Collections.Concurrent;
using NLog;
using Npgsql;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam;

public static class HutHelper
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static CardData ReadCardData(NpgsqlDataReader reader)
    {
        var cardData = new CardData
        {
            mAttributes = reader.GetFieldValue<byte[]>(reader.GetOrdinal("attributes")).ToList(),
            mCardStateId = (CardState)(byte)reader.GetInt16(reader.GetOrdinal("state_id")),
            mCardId = reader.GetInt64(reader.GetOrdinal("card_id")),
            mCardDbId = (uint)reader.GetInt32(reader.GetOrdinal("db_id")),
            mFormationId = (byte)reader.GetInt16(reader.GetOrdinal("formation_id")),
            mFREE = (byte)reader.GetInt16(reader.GetOrdinal("free")),
            mCareerRemaining = (byte)reader.GetInt16(reader.GetOrdinal("career_remaining")),
            mInjuryGames = (byte)reader.GetInt16(reader.GetOrdinal("injury_games")),
            mInjuryType = (byte)reader.GetInt16(reader.GetOrdinal("injury_type")),
            mMaxTrainingCardsCanApply = (byte)reader.GetInt16(reader.GetOrdinal("morale")),
            mNumberOfOwners = (byte)reader.GetInt16(reader.GetOrdinal("owners")),
            mPreferredPositionId = (byte)reader.GetInt16(reader.GetOrdinal("preferred_position_id")),
            mDiscardPrice = (short)reader.GetInt16(reader.GetOrdinal("discard_price")),
            mRareFlag = (byte)reader.GetInt16(reader.GetOrdinal("rare_flag")),
            mRating = (byte)reader.GetInt16(reader.GetOrdinal("rating")),
            mSalaryCap = reader.GetInt16(reader.GetOrdinal("salary_cap")),
            mListStats = reader.GetFieldValue<int[]>(reader.GetOrdinal("list_stats")).ToList(),
            mCardSubTypeId = (CardSubType)reader.GetInt16(reader.GetOrdinal("sub_type")),
            mDateIssued = (uint)reader.GetInt64(reader.GetOrdinal("date_issued")),
            mTeamId = (uint)reader.GetInt32(reader.GetOrdinal("team_id")),
            mListTrainingCards = reader.GetFieldValue<int[]>(reader.GetOrdinal("list_training_cards")).ToList(),
            mUsesRemaining = (byte)reader.GetInt16(reader.GetOrdinal("uses_remaining"))
        };
        return cardData;
    }

    public static async Task<ISTradeInfo> ReadTrade(NpgsqlDataReader reader, long readerUserId)
    {
        YourBid yourBid = await HutTradeManager.DetermineMyBidState(reader.GetInt64(reader.GetOrdinal("trade_id")), readerUserId);
        CardData cardData = (await HutManager.GetCard(reader.GetInt64(reader.GetOrdinal("card_id")))).Card;
        TradeState tradeState = (TradeState)reader.GetInt32(reader.GetOrdinal("trade_state"));
        int secondsLeft = tradeState == TradeState.CARDHOUSE_TRADESTATE_CLOSED || tradeState == TradeState.CARDHOUSE_TRADESTATE_EXPIRED ? 0 : reader.GetInt32(reader.GetOrdinal("expire_time"));
        return new ISTradeInfo
        {
            mBlazeUserId = reader.GetInt64(reader.GetOrdinal("user_id")),
            mCardData = cardData,
            mTradeId = reader.GetInt64(reader.GetOrdinal("trade_id")),
            mUserId = reader.GetInt64(reader.GetOrdinal("user_id")),
            mYourBidState = yourBid,
            mCardId = reader.GetInt64(reader.GetOrdinal("card_id")),
            mCardDbId = cardData.mCardDbId,
            mStartingPrice = reader.GetInt32(reader.GetOrdinal("starting_price")),
            mHighestBid = reader.GetInt32(reader.GetOrdinal("highest_bid")),
            mBuyOutPrice = reader.GetInt32(reader.GetOrdinal("buy_out_price")),
            mSellerName = reader.GetString(reader.GetOrdinal("seller_name")),
            mTradeState = (TradeState)reader.GetInt32(reader.GetOrdinal("trade_state")),
            mSecondsLeft = secondsLeft,
            // mSellerEstDate = Why?,
            mInbox = (tradeState == TradeState.CARDHOUSE_TRADESTATE_CLOSED || tradeState == TradeState.CARDHOUSE_TRADESTATE_EXPIRED) && yourBid == YourBid.CARDHOUSE_YOURBID_HIGHEST ? (byte)1 : (byte)0,
            mGlow = (tradeState == TradeState.CARDHOUSE_TRADESTATE_CLOSED || tradeState == TradeState.CARDHOUSE_TRADESTATE_EXPIRED) && yourBid == YourBid.CARDHOUSE_YOURBID_HIGHEST ? (byte)1 : (byte)0,
            mIsWatched = await HutTradeManager.IsWatching(readerUserId, reader.GetInt64(reader.GetOrdinal("trade_id"))) ? (byte)1 : (byte)0,
            mOfferPendingCount = (int)await HutTradeManager.ActiveOffersCount(reader.GetInt64(reader.GetOrdinal("trade_id"))),
        };
    }

    private static List<int>? _cachedLeagueIds;
    public static async Task<List<int>> GetAllLeagueIds()
    {
        if (_cachedLeagueIds != null) return _cachedLeagueIds;

        var leagueIds = new List<int>();

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = "SELECT DISTINCT leagueid FROM fcc_leagues ORDER BY leagueid ASC";

        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            leagueIds.Add(reader.GetInt32(0));
        }

        _cachedLeagueIds = leagueIds;
        return _cachedLeagueIds;
    }
    
    private static readonly ConcurrentDictionary<string, List<int>> CachedCardDbIds = new();
    public static async Task<List<int>> GetAllDistinctCardDbIds(string tableName)
    {
        if (CachedCardDbIds.TryGetValue(tableName, out var cached))
            return cached;

        var cardDbIds = new List<int>();

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        var sql = "SELECT DISTINCT carddbid FROM " + tableName + " ORDER BY carddbid";

        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            cardDbIds.Add(reader.GetInt32(0));
        }

        CachedCardDbIds[tableName] = cardDbIds;
        return cardDbIds;
    }

    public static async Task<ISOfferInfo> ReadOffer(NpgsqlDataReader reader)
    {
        var cardDataList = new List<CardData>();
        foreach (var cardId in reader.GetFieldValue<long[]>(reader.GetOrdinal("card_ids")).ToList())
        {
            var cardData = await HutManager.GetCard(cardId);
            cardDataList.Add(cardData.Card);
        }

        return new ISOfferInfo
        {
            mCardList = reader.GetFieldValue<long[]>(reader.GetOrdinal("card_ids")).ToList(),
            mCardDataList = cardDataList,
            mCredits = reader.GetInt32(reader.GetOrdinal("credits")),
            mOfferId = reader.GetInt64(reader.GetOrdinal("offer_id")),
            mOfferState = (OfferState)reader.GetInt32(reader.GetOrdinal("offer_state")),
            mTradeId = reader.GetInt64(reader.GetOrdinal("trade_id")),
        };
    }

    public static async Task<bool> Withdraw(long userId, int amount)
    {
        var generalIfo = await HutManager.GetGeneralInfo(userId);
        var currentCredits = generalIfo.Value.mCredits;

        if (currentCredits < amount) return false;
        if (amount <= 0) return true;

        var updated = generalIfo.Value with { mCredits = currentCredits - amount };

        await HutManager.SetGeneralInfo(updated, userId);
        await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.General);

        return true;
    }

    public static async Task<bool> Deposit(long userId, int amount)
    {
        if (amount <= 0)
        {
            Logger.Debug("Trying to deposit a negative amount! Balancing mistake in game end rewards?");
            return false;
        }

        var generalInfo = await HutManager.GetGeneralInfo(userId);

        var updated = generalInfo.Value with { mCredits = generalInfo.Value.mCredits + amount };

        await HutManager.SetGeneralInfo(updated, userId);
        await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.General);

        return true;
    }

    public enum Outcome
    {
        WIN,
        LOSS,
        OTL
    }

    public static async Task IncrementGeneralInfo(long userId, Outcome outcome)
    {
        var generalInfo = await HutManager.GetGeneralInfo(userId);
        var stats = new List<int>(generalInfo.Value.mStats);

        var index = outcome switch
        {
            Outcome.WIN => 8,
            Outcome.LOSS => 9,
            Outcome.OTL => 10,
            _ => throw new ArgumentOutOfRangeException(nameof(outcome))
        };

        stats[index]++;

        var updated = generalInfo.Value with { mStats = stats };

        await HutManager.SetGeneralInfo(updated, userId);
        await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.General);
    }

    public static short DetermineSalary(List<byte> attributes)
    {
        long sum = attributes[0] + attributes[1] + attributes[2] + attributes[3] + attributes[4];
        long cubed = sum * sum * sum;
        short result = (short)Math.Round(cubed / 500000.0);
        return (short)(result - 5);
    }
    
}