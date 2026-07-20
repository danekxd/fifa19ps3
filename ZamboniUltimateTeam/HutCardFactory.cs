using NLog;
using Npgsql;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam;

public static class HutCardFactory
{
    
    public static async Task<CardData> CreateNonPlayerCard(long owner, uint dbId, CardSubType cardSubType, byte formationId = 0)
    {
        CardState cardState = CardState.CARDHOUSE_CARDSTATE_FREE;
        DeckType deckType = DeckType.CARDHOUSE_DECK_UNASSIGNED;
        if (CardHouseComponent.TrophyTypes.Contains(cardSubType))
        {
            deckType = DeckType.CARDHOUSE_DECK_STICKERBOOK;
        }

        var cardData = new CardData()
        {
            mAttributes = new List<byte>(),
            mCardStateId = cardState,
            mCardId = 0,
            mCardDbId = dbId,
            mFormationId = formationId,
            mFREE = 0,
            mCareerRemaining = 0,
            mInjuryGames = 0,
            mInjuryType = 0,
            mMaxTrainingCardsCanApply = 0,
            mNumberOfOwners = 0,
            mPreferredPositionId = (byte)cardSubType,
            mDiscardPrice = 0,
            mRareFlag = 0,
            mRating = 0,
            mSalaryCap = 0,
            mListStats = new List<int>(),
            mCardSubTypeId = cardSubType,
            mDateIssued = UltimateTeam.TimeNowSeconds(),
            mTeamId = (uint)await UltimateDatabase.TeamIdFromDbId(dbId),
            mListTrainingCards = new List<int>(),
            mUsesRemaining = (byte)(CardHouseComponent.TrophyTypes.Contains(cardSubType) ? 1 : 0)
        };
        return await CreateOrUpdateCard(cardData, owner, deckType);
    }

    public static async Task<CardData> CreatePlayerCard(long owner, uint dbId)
    {
        var staticCardData = await UltimateDatabase.GetPlayerCardDataByDbId(dbId);
        if (!staticCardData.HasValue) throw new Exception();
        return await CreateOrUpdateCard(staticCardData.Value, owner, DeckType.CARDHOUSE_DECK_UNASSIGNED);
    }

    public static async Task<CardData> CreateOrUpdateCard(CardData cardData, long ownerUserId, DeckType? deckType = null)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        string cardIdValue = cardData.mCardId == 0 ? "DEFAULT" : "@card_id";
        bool updateDeck = deckType.HasValue;

        string sql = $@"
        INSERT INTO hut_cards (
            card_id, user_id, attributes, state_id, db_id, formation_id, 
            free, career_remaining, injury_games, injury_type, 
            morale, owners, preferred_position_id, discard_price, 
            rare_flag, rating, salary_cap,
            list_stats, sub_type, date_issued,
            team_id, list_training_cards, uses_remaining
            {(updateDeck ? ", deck_type" : "")} 
        ) 
        VALUES (
            {cardIdValue}, @user_id, @attributes, @state_id, @db_id, @formation_id, 
            @free, @career_remaining, @injury_games, @injury_type, 
            @morale, @owners, @preferred_position_id, @discard_price, 
            @rare_flag, @rating, @salary_cap,
            @list_stats, @sub_type, @date_issued, @team_id, @list_training_cards, 
            @uses_remaining
            {(updateDeck ? ", @deck_type" : "")}
        )
        ON CONFLICT (card_id) DO UPDATE SET
            user_id = EXCLUDED.user_id,
            attributes = EXCLUDED.attributes,
            state_id = EXCLUDED.state_id,
            db_id = EXCLUDED.db_id,
            formation_id = EXCLUDED.formation_id,
            free = EXCLUDED.free,
            career_remaining = EXCLUDED.career_remaining,
            injury_games = EXCLUDED.injury_games,
            injury_type = EXCLUDED.injury_type,
            morale = EXCLUDED.morale,
            owners = EXCLUDED.owners,
            preferred_position_id = EXCLUDED.preferred_position_id,
            discard_price = EXCLUDED.discard_price,
            rare_flag = EXCLUDED.rare_flag,
            rating = EXCLUDED.rating,
            salary_cap = EXCLUDED.salary_cap,
            list_stats = EXCLUDED.list_stats,
            sub_type = EXCLUDED.sub_type,
            team_id = EXCLUDED.team_id,
            list_training_cards = EXCLUDED.list_training_cards,
            uses_remaining = EXCLUDED.uses_remaining
            {(updateDeck ? ", deck_type = EXCLUDED.deck_type" : "")}
        RETURNING card_id;";

        await using var cmd = new NpgsqlCommand(sql, conn);

        if (cardData.mCardId != 0) cmd.Parameters.AddWithValue("card_id", cardData.mCardId);

        cmd.Parameters.AddWithValue("user_id", ownerUserId);
        cmd.Parameters.AddWithValue("attributes", cardData.mAttributes.Select(b => (short)b).ToArray());
        cmd.Parameters.AddWithValue("state_id", (int)cardData.mCardStateId);
        cmd.Parameters.AddWithValue("db_id", (long)cardData.mCardDbId);
        cmd.Parameters.AddWithValue("formation_id", (int)cardData.mFormationId);
        cmd.Parameters.AddWithValue("free", (int)cardData.mFREE);
        cmd.Parameters.AddWithValue("career_remaining", (int)cardData.mCareerRemaining);
        cmd.Parameters.AddWithValue("injury_games", (short)cardData.mInjuryGames);
        cmd.Parameters.AddWithValue("injury_type", (short)cardData.mInjuryType);
        cmd.Parameters.AddWithValue("morale", (short)cardData.mMaxTrainingCardsCanApply);
        cmd.Parameters.AddWithValue("owners", (short)cardData.mNumberOfOwners);
        cmd.Parameters.AddWithValue("preferred_position_id", (int)cardData.mPreferredPositionId);
        cmd.Parameters.AddWithValue("discard_price", (int)cardData.mDiscardPrice);
        cmd.Parameters.AddWithValue("rare_flag", (int)cardData.mRareFlag);
        cmd.Parameters.AddWithValue("rating", (int)cardData.mRating);
        cmd.Parameters.AddWithValue("salary_cap", (int)cardData.mSalaryCap);
        cmd.Parameters.AddWithValue("list_stats", cardData.mListStats.ToArray());
        cmd.Parameters.AddWithValue("list_training_cards", cardData.mListTrainingCards.ToArray());
        cmd.Parameters.AddWithValue("sub_type", (int)cardData.mCardSubTypeId);
        cmd.Parameters.AddWithValue("date_issued", (long)UltimateTeam.TimeNowSeconds());
        cmd.Parameters.AddWithValue("team_id", (int)cardData.mTeamId);
        cmd.Parameters.AddWithValue("uses_remaining", (int)cardData.mUsesRemaining);
        if (updateDeck)
        {
            cmd.Parameters.AddWithValue("deck_type", (int)deckType.Value);
        }

        cardData.mCardId = (long)await cmd.ExecuteScalarAsync();

        return cardData;
    }
}