using System.Text;
using Npgsql;
using ZamboniUltimateTeam.Card;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam;

public static class UltimateDatabase
{
    public static string? ConnectionString;

    public static void CreateTables()
    {
        CreateTradeInfoTable();
        CreateOfferInfoTable();
        CreateWatchingTable();

        CreateHutGamerInfoTable();
        CreateHutSquadInfoTable();
        CreateHutVersionInfoTable();
        CreateHutGeneralInfoTable();

        CreateHutCardsTable();

        CreateHutTournamentsTable();

        CreateHutNameReservationsTable();
    }

    private static void CreateHutGeneralInfoTable()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS hut_general_info (
                    user_id BIGINT PRIMARY KEY,
                    pucks INTEGER,
                    stats INTEGER[] DEFAULT '{}'
                );";

        using var cmd = new NpgsqlCommand(createTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    private static void CreateHutVersionInfoTable()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS hut_version_info (
                    user_id BIGINT PRIMARY KEY,
                    escrow_version INTEGER,
                    general_version INTEGER,
                    unassigned_version INTEGER
                );";

        using var cmd = new NpgsqlCommand(createTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    private static void CreateHutGamerInfoTable()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS hut_gamer_info (
                    user_id BIGINT PRIMARY KEY,
                    custom_tactics VARCHAR,
                    team_formation INTEGER,
                    kick_takers VARCHAR,
                    lineup VARCHAR,
                    logo_url VARCHAR,
                    team_name VARCHAR,
                    playoffs_qualified INTEGER,
                    playoff_won INTEGER,
                    quick_tactics VARCHAR,
                    special_packs_bought INTEGER,
                    team_abbreviation VARCHAR,
                    tournaments VARCHAR,
                    tppl INTEGER,
                    trophies VARCHAR
                );";

        using var cmd = new NpgsqlCommand(createTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    private static void CreateHutSquadInfoTable()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS hut_squad_info (
                    squad_id SERIAL PRIMARY KEY,
                    user_id BIGINT,
                    chemistry INTEGER,
                    formation_id INTEGER,
                    lines INTEGER[] DEFAULT '{}',
                    manager BIGINT,
                    squad_name VARCHAR,
                    players BIGINT[] DEFAULT '{}',
                    rating_def INTEGER,
                    rating_gk INTEGER,
                    rating_off INTEGER,
                    star_rating INTEGER,
                    active BOOLEAN
                );";

        using var cmd = new NpgsqlCommand(createTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    private static void CreateHutCardsTable()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS hut_cards (
                    card_id BIGSERIAL PRIMARY KEY,
                    user_id BIGINT,
                    
                    attributes SMALLINT[] DEFAULT '{}',
                    state_id SMALLINT,
                    db_id INTEGER,
                    formation_id SMALLINT,
                    free SMALLINT,
                    career_remaining SMALLINT,
                    injury_games SMALLINT,
                    injury_type SMALLINT,
                    morale SMALLINT, --mMaxTrainingCardsCanApply/Potential
                    owners SMALLINT,
                    preferred_position_id SMALLINT,
                    discard_price SMALLINT,
                    rare_flag SMALLINT,
                    rating SMALLINT,
                    salary_cap INTEGER,
                    list_stats INTEGER[] DEFAULT '{}',
                    sub_type SMALLINT,
                    date_issued BIGINT,
                    team_id INTEGER,
                    list_training_cards INTEGER[] DEFAULT '{}',
                    uses_remaining SMALLINT,
                    deck_type INTEGER DEFAULT 1
                );";

        using var cmd = new NpgsqlCommand(createTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    private static void CreateHutTournamentsTable()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS hut_tournaments (
                    user_id BIGINT,
                    tournament_type INTEGER,
                    tournament_id INTEGER,
                    blaze_tournament_id  INTEGER,
                    active BOOLEAN,
                    tournament_data BYTEA,
                    PRIMARY KEY (user_id, tournament_type)
                );";

        using var cmd = new NpgsqlCommand(createTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    private static void CreateHutNameReservationsTable()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS hut_name_reservations (
                    user_id BIGINT,
                    user_name VARCHAR,
                    team_name VARCHAR,
                    team_abbreviation VARCHAR,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    deleted_at TIMESTAMP,
                    set_free BOOLEAN DEFAULT false
                );";

        using var cmd = new NpgsqlCommand(createTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    private static void CreateTradeInfoTable()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS hut_trade_info (
                    trade_id BIGSERIAL PRIMARY KEY,
                    user_id BIGINT,
                    card_id BIGINT,
                    starting_price INTEGER,
                    highest_bid INTEGER DEFAULT 0,
                    buy_out_price INTEGER,
                    seller_name VARCHAR,
                    trade_state INTEGER,
                    duration_seconds INTEGER,
                    created_at_seconds BIGINT
                );";

        using var cmd = new NpgsqlCommand(createTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    private static void CreateOfferInfoTable()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS hut_offer_info (
                    offer_id BIGSERIAL PRIMARY KEY,
                    trade_id BIGINT,
                    user_id BIGINT,
                    offer_state INTEGER,
                    credits INTEGER,
                    card_ids BIGINT[] DEFAULT '{}',
                    created_at_seconds BIGINT
                );";

        using var cmd = new NpgsqlCommand(createTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    private static void CreateWatchingTable()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        const string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS hut_watching (
                user_id BIGINT,
                trade_id BIGINT,
                PRIMARY KEY (user_id, trade_id)
            );";

        using var cmd = new NpgsqlCommand(createTableQuery, conn);
        cmd.ExecuteNonQuery();
    }

    public static async Task<CardData?> GetPlayerCardDataByDbId(uint cardDbId)
    {
        const string sql = "SELECT * FROM fcc_playercards WHERE carddbid = @dbid LIMIT 1";

        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("dbid", (int)cardDbId);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            short rating = reader.GetInt16(reader.GetOrdinal("rating"));
            List<byte> attributes = new List<byte>
            {
                reader.GetByte(reader.GetOrdinal("attribute1")),
                reader.GetByte(reader.GetOrdinal("attribute2")),
                reader.GetByte(reader.GetOrdinal("attribute3")),
                reader.GetByte(reader.GetOrdinal("attribute4")),
                reader.GetByte(reader.GetOrdinal("attribute5")),
                reader.GetByte(reader.GetOrdinal("attribute6")),
                reader.GetByte(reader.GetOrdinal("attribute7")),
                reader.GetByte(reader.GetOrdinal("attribute8")),
            };
            return new CardData
            {
                mAttributes = attributes,
                mCardStateId = CardState.CARDHOUSE_CARDSTATE_FREE,
                mCardDbId = cardDbId,
                mFormationId = reader.GetByte(reader.GetOrdinal("formationid")),
                // mFREE = 40, //Does this has meaning?
                mCareerRemaining = reader.GetByte(reader.GetOrdinal("attribute8")),
                mInjuryGames = reader.GetByte(reader.GetOrdinal("injuryduration")),
                mInjuryType = reader.GetByte(reader.GetOrdinal("injury")),
                mNumberOfOwners = 1,
                mMaxTrainingCardsCanApply = reader.GetByte(reader.GetOrdinal("attribute7")),
                mPreferredPositionId = reader.GetByte(reader.GetOrdinal("preferredposition")),
                mDiscardPrice = 0,
                mRareFlag = reader.GetByte(reader.GetOrdinal("rare")),
                mRating = (byte)rating,
                mSalaryCap = HutHelper.DetermineSalary(attributes),
                mListStats = new List<int>
                {
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                },
                mCardSubTypeId = (CardSubType)reader.GetInt16(reader.GetOrdinal("fieldpos")),
                mDateIssued = UltimateTeam.TimeNowSeconds(),
                mTeamId = (uint)reader.GetInt32(reader.GetOrdinal("teamid")),
                mListTrainingCards = new List<int>(),
                mUsesRemaining = (byte)Random.Shared.Next(6, 13),
            };
        }

        return null;
    }

    public static async Task<int> TeamIdFromDbId(uint dbId)
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        const string sql = @"
        SELECT teamid FROM fcc_badges WHERE carddbid = @carddbid
        UNION ALL
        SELECT teamid FROM fcc_kitcards WHERE carddbid = @carddbid
        LIMIT 1;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("carddbid", (int)dbId);

        var result = await cmd.ExecuteScalarAsync();

        if (result != null && result != DBNull.Value)
        {
            return Convert.ToInt32(result);
        }

        return 0;
    }

    public static async Task<HutTrainingCard> GetTrainingCardByDbIdAsync(uint cardDbId)
    {
        const string sql = "SELECT * FROM fcc_trainingcards WHERE carddbid = @cardDbId";

        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        await using var command = new NpgsqlCommand(sql, conn);
        command.Parameters.AddWithValue("cardDbId", (int)cardDbId);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new HutTrainingCard
            {
                CardDbId = (uint)reader.GetInt32(0),
                CardSubType = reader.GetInt32(1),
                WeightRare = reader.GetInt32(2),
                CardAssetId = reader.GetInt32(3),
                Description = reader.GetString(4),
                Amount = reader.GetInt32(5),
                Rating = reader.GetInt32(6),
                AttributeSlot = reader.GetInt32(7),
                IndexedConsumableId = reader.GetInt32(8)
            };
        }

        throw new Exception();
    }

    public static async Task<HutContractCard> GetContractCardByDbIdAsync(uint cardDbId)
    {
        const string sql = "SELECT * FROM fcc_contractcards WHERE carddbid = @cardDbId";

        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        await using var command = new NpgsqlCommand(sql, conn);
        command.Parameters.AddWithValue("cardDbId", (int)cardDbId);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new HutContractCard
            {
                CardDbId = (uint)reader.GetInt32(0),
                WeightRare = reader.GetInt32(1),
                Value = reader.GetInt32(2),
            };
        }

        throw new Exception();
    }

    public static async Task<HealingCard> GetHealingCardByDbIdAsync(uint cardDbId)
    {
        const string sql = "SELECT * FROM fcc_healingcards WHERE carddbid = @cardDbId";

        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        await using var command = new NpgsqlCommand(sql, conn);
        command.Parameters.AddWithValue("cardDbId", (int)cardDbId);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new HealingCard
            {
                CardDbId = (uint)reader.GetInt32(0),
                CardSubType = reader.GetInt32(1),
                WeightRare = reader.GetInt32(2),
                Amount = reader.GetInt32(3)
            };
        }

        throw new Exception();
    }
    
    public static async Task<HutKitCard> GetKitCardByDbIdAsync(uint cardDbId)
    {
        const string sql = "SELECT * FROM fcc_kitcards WHERE carddbid = @cardDbId";

        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        await using var command = new NpgsqlCommand(sql, conn);
        command.Parameters.AddWithValue("cardDbId", (int)cardDbId);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new HutKitCard
            {
                CardDbId = (uint)reader.GetInt32(0),
                Alternative = reader.GetBoolean(1),
                TeamId = (uint)reader.GetInt32(2),
                IsAway = reader.GetBoolean(3),
            };
        }

        throw new Exception();
    }

    public static async Task<List<HutKitCard>> GetKitCards(bool? isAway, bool? isRare)
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        var sql = new StringBuilder(@"
            SELECT carddbid, alternative, teamid, isaway
            FROM fcc_kitcards
            WHERE 1=1");

        if (isAway.HasValue) sql.Append(" AND isaway=@isaway");
        if (isRare.HasValue) sql.Append(" AND alternative=@israre");

        await using var command = new NpgsqlCommand(sql.ToString(), conn);

        if (isAway.HasValue) command.Parameters.AddWithValue("isaway", isAway.Value);
        if (isRare.HasValue) command.Parameters.AddWithValue("israre", isRare.Value);

        await using var reader = await command.ExecuteReaderAsync();

        var returningList = new List<HutKitCard>();

        while (await reader.ReadAsync())
        {
            returningList.Add(new HutKitCard
            {
                CardDbId = (uint)reader.GetInt64(0),
                Alternative = reader.GetBoolean(1),
                TeamId = (uint)reader.GetInt32(2),
                IsAway = reader.GetBoolean(3),
            });
        }

        return returningList;
    }

    public static async Task AwardAll(long userId)
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        var sql = new StringBuilder(@"
            SELECT carddbid
            FROM fcc_playercards
            WHERE 1=1");

        await using var command = new NpgsqlCommand(sql.ToString(), conn);

        await using var reader = await command.ExecuteReaderAsync();


        while (await reader.ReadAsync())
        {
            var existing = await HutManager.GetCard((uint)reader.GetInt32(0), userId);
            if (existing.Card.mCardId != 0)
            {
                await HutCardFactory.CreateOrUpdateCard(existing.Card, userId, DeckType.CARDHOUSE_DECK_STICKERBOOK);
            }
            else
            {
                await HutCardFactory.CreatePlayerCard(userId, (uint)reader.GetInt32(0));
            }
        }
    }
}