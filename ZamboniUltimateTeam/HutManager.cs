using System.Text;
using NLog;
using Npgsql;
using ZamboniUltimateTeam.Requests;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam;

public static class HutManager
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static async Task<GamerInfo?> GetGamerInfo(long userId)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = "SELECT * FROM hut_gamer_info WHERE user_id = @uid;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("uid", userId);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new GamerInfo
            {
                mCustomTactics = reader.GetString(reader.GetOrdinal("custom_tactics")),
                mTeamFormation = (uint)reader.GetInt32(reader.GetOrdinal("team_formation")),
                mKickTakers = reader.GetString(reader.GetOrdinal("kick_takers")),
                mLineup = reader.GetString(reader.GetOrdinal("lineup")),
                mLogoUrl = reader.GetString(reader.GetOrdinal("logo_url")),
                mTeamName = reader.GetString(reader.GetOrdinal("team_name")),
                mPlayoffsQualified = (uint)reader.GetInt32(reader.GetOrdinal("playoffs_qualified")),
                mPlayoffWon = (uint)reader.GetInt32(reader.GetOrdinal("playoff_won")),
                mQuickTactics = reader.GetString(reader.GetOrdinal("quick_tactics")),
                mSpecialPacksBought = (uint)reader.GetInt32(reader.GetOrdinal("special_packs_bought")),
                mTeamAbbreviation = reader.GetString(reader.GetOrdinal("team_abbreviation")),
                mTournaments = reader.GetString(reader.GetOrdinal("tournaments")),
                mTPPL = (uint)reader.GetInt32(reader.GetOrdinal("tppl")),
                mTrophies = reader.GetString(reader.GetOrdinal("trophies"))
            };
        }

        return null;
    }

    public static async Task SetGamerInfo(GamerInfo gamerInfo, long userId)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = @"
        INSERT INTO hut_gamer_info (
            user_id, custom_tactics, team_formation, kick_takers, lineup, 
            logo_url, team_name, playoffs_qualified, playoff_won, 
            quick_tactics, special_packs_bought, team_abbreviation, 
            tournaments, tppl, trophies
        ) 
        VALUES (
            @user_id, @custom_tactics, @team_formation, @kick_takers, @lineup, 
            @logo_url, @team_name, @playoffs_qualified, @playoff_won, 
            @quick_tactics, @special_packs_bought, @team_abbreviation, 
            @tournaments, @tppl, @trophies
        )
        ON CONFLICT (user_id) DO UPDATE SET
            custom_tactics = EXCLUDED.custom_tactics,
            team_formation = EXCLUDED.team_formation,
            kick_takers = EXCLUDED.kick_takers,
            lineup = EXCLUDED.lineup,
            logo_url = EXCLUDED.logo_url,
            team_name = EXCLUDED.team_name,
            playoffs_qualified = EXCLUDED.playoffs_qualified,
            playoff_won = EXCLUDED.playoff_won,
            quick_tactics = EXCLUDED.quick_tactics,
            special_packs_bought = EXCLUDED.special_packs_bought,
            team_abbreviation = EXCLUDED.team_abbreviation,
            tournaments = EXCLUDED.tournaments,
            tppl = EXCLUDED.tppl,
            trophies = EXCLUDED.trophies;";

        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("custom_tactics", gamerInfo.mCustomTactics);
        cmd.Parameters.AddWithValue("team_formation", (int)gamerInfo.mTeamFormation);
        cmd.Parameters.AddWithValue("kick_takers", gamerInfo.mKickTakers);
        cmd.Parameters.AddWithValue("lineup", gamerInfo.mLineup);
        cmd.Parameters.AddWithValue("logo_url", gamerInfo.mLogoUrl);
        cmd.Parameters.AddWithValue("team_name", gamerInfo.mTeamName ?? "");
        cmd.Parameters.AddWithValue("playoffs_qualified", (int)gamerInfo.mPlayoffsQualified);
        cmd.Parameters.AddWithValue("playoff_won", (int)gamerInfo.mPlayoffWon);
        cmd.Parameters.AddWithValue("quick_tactics", gamerInfo.mQuickTactics);
        cmd.Parameters.AddWithValue("special_packs_bought", (int)gamerInfo.mSpecialPacksBought);
        cmd.Parameters.AddWithValue("team_abbreviation", gamerInfo.mTeamAbbreviation);
        cmd.Parameters.AddWithValue("tournaments", gamerInfo.mTournaments);
        cmd.Parameters.AddWithValue("tppl", (int)gamerInfo.mTPPL);
        cmd.Parameters.AddWithValue("trophies", gamerInfo.mTrophies);

        await cmd.ExecuteNonQueryAsync();
    }

    public static async Task<GeneralInfo?> GetGeneralInfo(long userId)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = "SELECT * FROM hut_general_info WHERE user_id = @uid;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("uid", userId);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new GeneralInfo
            {
                mCredits = reader.GetInt32(reader.GetOrdinal("pucks")),
                mStats = reader.GetFieldValue<int[]>(reader.GetOrdinal("stats")).ToList(),
            };
        }

        return null;
    }

    public static async Task<GeneralInfo> SetGeneralInfo(GeneralInfo generalInfo, long userId)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = @"
        INSERT INTO hut_general_info (
            user_id, pucks, stats
        ) 
        VALUES (
            @user_id, @pucks, @stats
        )
        ON CONFLICT (user_id) DO UPDATE SET
            pucks = EXCLUDED.pucks,
            stats = EXCLUDED.stats;";

        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("pucks", generalInfo.mCredits);
        cmd.Parameters.AddWithValue("stats", generalInfo.mStats.ToArray());

        await cmd.ExecuteNonQueryAsync();

        await IncrementVersionInfo(userId, VersionType.General);

        return generalInfo;
    }

    private static int chngDebugCounter = 0;

    public static async Task<List<SquadInfo>> GetSquadList(long userId)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        List<SquadInfo> squadList = new();

        const string sql = @"
            SELECT squad_id 
            FROM hut_squad_info 
            WHERE user_id = @user_id 
            ORDER BY active DESC;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("user_id", userId);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            SquadInfo? squadInfo = await GetSquad(userId, reader.GetInt32(reader.GetOrdinal("squad_id")), false);
            if (squadInfo != null) squadList.Add(squadInfo.Value);
        }

        return squadList;
    }

    public static async Task<SquadInfo?> GetSquad(long userId, int squadId, bool makeActive)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = @"
            WITH updated AS (
                UPDATE hut_squad_info
                SET active = (squad_id = @squad_id)
                WHERE user_id = @user_id
                AND @make_active = true
            )
            SELECT s.*, g.team_abbreviation 
            FROM hut_squad_info AS s 
            INNER JOIN hut_gamer_info AS g ON s.user_id = g.user_id 
            WHERE s.user_id = @user_id
            AND s.squad_id = @squad_id;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("squad_id", squadId);
        cmd.Parameters.AddWithValue("make_active", makeActive);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var playerIds = reader.GetFieldValue<List<long>>(reader.GetOrdinal("players"));

            var playersOrdered = (await Task.WhenAll(
                playerIds.Select(cardId => GetCard(cardId, userId, DeckType.CARDHOUSE_DECK_STICKERBOOK))
            )).Select(result => result.Card).ToList();

            var logoTask = GetCardList(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardState.CARDHOUSE_CARDSTATE_ACTIVE_BADGE);
            var homeJerseyTask = GetCardList(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardState.CARDHOUSE_CARDSTATE_ACTIVE_HOME_KIT);
            var awayJerseyTask = GetCardList(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardState.CARDHOUSE_CARDSTATE_ACTIVE_AWAY_KIT);
            var stadiumTask = GetCardList(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardState.CARDHOUSE_CARDSTATE_ACTIVE_STADIUM);

            await Task.WhenAll(
                logoTask,
                homeJerseyTask,
                awayJerseyTask,
                stadiumTask
            );

            var logoCardDbId = (await logoTask).FirstOrDefault().mCardDbId;
            var homeJerseyDbId = (await homeJerseyTask).FirstOrDefault().mCardDbId;
            var awayJerseyDbId = (await awayJerseyTask).FirstOrDefault().mCardDbId;
            var stadiumDbId = (await stadiumTask).FirstOrDefault().mCardDbId;

            return new SquadInfo()
            {
                mChemistry = (uint)reader.GetInt32(reader.GetOrdinal("chemistry")),
                mCHNG = 0,
                mFormationId = (uint)reader.GetInt32(reader.GetOrdinal("formation_id")),
                mJerseyAwayDbId = awayJerseyDbId,
                mJerseyHomeDbId = homeJerseyDbId,
                mLines = reader.GetFieldValue<int[]>(reader.GetOrdinal("lines")).ToList(),
                mManager = (await GetCard(reader.GetInt64(reader.GetOrdinal("manager")))).Card,
                mSquadName = reader.GetString(reader.GetOrdinal("squad_name")),
                mPlayers = playersOrdered,
                mStarRating = (uint)reader.GetInt32(reader.GetOrdinal("star_rating")),
                mSquadId = reader.GetInt32(reader.GetOrdinal("squad_id")),
                mStadiumDbId = stadiumDbId,
                mTeamAbbreviation = reader.GetString(reader.GetOrdinal("team_abbreviation")),
                mLogoCardDbId = logoCardDbId
            };
        }

        return null;
    }

    public static async Task<List<OfflineOpponentTeam>> GetAllSquadsAsOfflineOpponents(long excludedUserId)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = "SELECT * FROM hut_squad_info WHERE active = true ORDER BY RANDOM() LIMIT 10;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        var offlineTeams = new List<OfflineOpponentTeam>();
        while (await reader.ReadAsync())
        {
            var loopUserId = reader.GetInt64(reader.GetOrdinal("user_id"));
            if (loopUserId == excludedUserId) continue;

            var logoCardResult = await GetCardList(loopUserId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardState.CARDHOUSE_CARDSTATE_ACTIVE_BADGE);
            var logoCardDbId = logoCardResult.FirstOrDefault().mCardDbId;

            var gamerInfo = await GetGamerInfo(loopUserId); //Do this more efficiently
            var abbreviation = gamerInfo.Value.mTeamAbbreviation;

            offlineTeams.Add(new OfflineOpponentTeam
            {
                mLogoDbId = logoCardDbId,
                mOpponentId = loopUserId,
                mRatingDefensive = (byte)reader.GetInt32(reader.GetOrdinal("rating_def")),
                mRatingGoalie = (byte)reader.GetInt32(reader.GetOrdinal("rating_gk")),
                mRatingOffensive = (byte)reader.GetInt32(reader.GetOrdinal("rating_off")),
                mStarRating = (byte)reader.GetInt32(reader.GetOrdinal("star_rating")),
                mSquadId = reader.GetInt32(reader.GetOrdinal("squad_id")),
                mTeamAbbreviation = abbreviation,
                mTeamName = reader.GetString(reader.GetOrdinal("squad_name")),
                mTOPT = 10 //What is this
            });
        }

        return offlineTeams;
    }

    public static async Task<VersionInfo?> GetVersionInfo(long userId)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = "SELECT * FROM hut_version_info WHERE user_id = @user_id;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("user_id", userId);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new VersionInfo
            {
                mVersionEscrow = (uint)reader.GetInt32(reader.GetOrdinal("escrow_version")),
                mVersionGeneral = (uint)reader.GetInt32(reader.GetOrdinal("general_version")),
                mVersionUnassigned = (uint)reader.GetInt32(reader.GetOrdinal("unassigned_version")),
            };
        }

        return null;
    }

    public static async Task<VersionInfo> CreateVersionInfo(VersionInfo versionInfo, long userId)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = @"
        INSERT INTO hut_version_info (
            user_id, escrow_version, general_version, unassigned_version
        ) 
        VALUES (
            @user_id, @escrow_version, @general_version, @unassigned_version
        );";

        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("escrow_version", (int)versionInfo.mVersionEscrow);
        cmd.Parameters.AddWithValue("general_version", (int)versionInfo.mVersionGeneral);
        cmd.Parameters.AddWithValue("unassigned_version", (int)versionInfo.mVersionUnassigned);

        await cmd.ExecuteNonQueryAsync();
        return versionInfo;
    }

    public enum VersionType
    {
        Escrow,
        General,
        Unassigned
    }

    public static async Task<VersionInfo> IncrementVersionInfo(long userId, VersionType type)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = @"
        INSERT INTO hut_version_info (user_id, escrow_version, general_version, unassigned_version)
        VALUES (@uid, 1, 1, 1)
        ON CONFLICT (user_id) DO UPDATE SET
            escrow_version = CASE WHEN @type = 'Escrow' THEN hut_version_info.escrow_version + 1 ELSE hut_version_info.escrow_version END,
            general_version = CASE WHEN @type = 'General' THEN hut_version_info.general_version + 1 ELSE hut_version_info.general_version END,
            unassigned_version = CASE WHEN @type = 'Unassigned' THEN hut_version_info.unassigned_version + 1 ELSE hut_version_info.unassigned_version END
        RETURNING escrow_version, general_version, unassigned_version;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("uid", userId);
        cmd.Parameters.AddWithValue("type", type.ToString());

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new VersionInfo
            {
                mVersionEscrow = (uint)reader.GetInt32(reader.GetOrdinal("escrow_version")),
                mVersionGeneral = (uint)reader.GetInt32(reader.GetOrdinal("general_version")),
                mVersionUnassigned = (uint)reader.GetInt32(reader.GetOrdinal("unassigned_version"))
            };
        }

        return new VersionInfo { mVersionEscrow = 1, mVersionGeneral = 1, mVersionUnassigned = 1 };
    }

    public static async Task<List<CardData>> GetCardList(long userId, DeckType deckType, CardState cardState)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = "SELECT * FROM hut_cards WHERE user_id = @user_id AND deck_type = @deck_type AND state_id = @state_id;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("deck_type", (int)deckType);
        cmd.Parameters.AddWithValue("state_id", (short)cardState);

        await using var reader = await cmd.ExecuteReaderAsync();

        List<CardData> cardDataList = new List<CardData>();

        while (await reader.ReadAsync())
        {
            cardDataList.Add(HutHelper.ReadCardData(reader));
        }

        return cardDataList;
    }

    public static async Task<List<FriendHistoryEntry>> QueryTeamStats(int limit)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = @"
            SELECT g.user_id, g.stats, n.user_name
            FROM hut_general_info g
            LEFT JOIN hut_name_reservations n 
                ON g.user_id = n.user_id 
                AND n.deleted_at IS NULL
            ORDER BY g.stats[9] DESC
            LIMIT @limit
            ;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("limit", limit);

        await using var reader = await cmd.ExecuteReaderAsync();

        List<FriendHistoryEntry> retList = new List<FriendHistoryEntry>();
        string format = "D" + limit.ToString().Length;

        int statsOrdinal = reader.GetOrdinal("stats");
        int userIdOrdinal = reader.GetOrdinal("user_id");
        int nameOrdinal = reader.GetOrdinal("user_name");

        int position = 1;

        while (await reader.ReadAsync())
        {
            int[] stats = reader.GetFieldValue<int[]>(statsOrdinal);

            string rawName = reader.IsDBNull(nameOrdinal) ? "Unknown User" : reader.GetString(nameOrdinal);

            retList.Add(new FriendHistoryEntry
            {
                mLosses = (short)stats[9],
                mOpponentId = (uint)reader.GetInt64(userIdOrdinal),
                mOpponentName = $"{position.ToString(format)}. {rawName}",
                mOverTimeLosses = (short)stats[10],
                mWins = (short)stats[8]
            });
            position++;
        }

        return retList;
    }

    public static async Task<List<FriendGameEntry>> QueryFriendGameList(long userId, GetFriendGameListRequest request)
    {
        var games = new List<FriendGameEntry>();

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        const string sql = @"
        SELECT 
            mine.score, 
            opp.score as opp_score, 
            mine.disc,
            mine.quit,
            g.ot,
            g.shootout,
            mine.created_at
        FROM hut_reports_l mine
        INNER JOIN hut_reports_l opp ON mine.game_id = opp.game_id AND mine.user_id != opp.user_id
        INNER JOIN games_l g ON mine.game_id = g.game_id
        WHERE mine.user_id = @userId
          AND opp.user_id = @opponentId
          AND mine.score IS NOT NULL 
          AND opp.score IS NOT NULL
        ORDER BY mine.created_at DESC
        LIMIT @limit;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("userId", userId);
        cmd.Parameters.AddWithValue("opponentId", request.mOpponentId);
        cmd.Parameters.AddWithValue("limit", (int)request.mMaxResults);

        await using var reader = await cmd.ExecuteReaderAsync();

        int myScoreOrd = reader.GetOrdinal("score");
        int oppScoreOrd = reader.GetOrdinal("opp_score");
        int discOrd = reader.GetOrdinal("disc");
        int quitOrd = reader.GetOrdinal("quit");
        int otOrd = reader.GetOrdinal("ot");
        int soOrd = reader.GetOrdinal("shootout");
        int timeOrd = reader.GetOrdinal("created_at");

        while (await reader.ReadAsync())
        {
            if (reader.IsDBNull(myScoreOrd) || reader.IsDBNull(oppScoreOrd)) continue;

            int myScore = reader.GetInt32(myScoreOrd);
            int oppScore = reader.GetInt32(oppScoreOrd);
            bool isDisc = reader.GetInt32(discOrd) == 1;
            bool isQuit = reader.GetInt32(quitOrd) == 1;
            bool wentOverTime = reader.GetInt32(otOrd) == 1 || reader.GetInt32(soOrd) == 1;

            CardHouseGameResult finalResult;

            if (isDisc)
                finalResult = CardHouseGameResult.GAME_RESULT_DISC;
            else if (isQuit)
                finalResult = CardHouseGameResult.GAME_RESULT_QUIT;
            else if (myScore > oppScore)
                finalResult = CardHouseGameResult.GAME_RESULT_WIN;
            else if (wentOverTime)
                finalResult = CardHouseGameResult.GAME_RESULT_OT;
            else
                finalResult = CardHouseGameResult.GAME_RESULT_LOSS;

            games.Add(new FriendGameEntry
            {
                mMyGoals = (byte)myScore,
                mOpponentsGoals = (byte)oppScore,
                mResult = finalResult,
                mPlayedAt = (uint)((DateTimeOffset)reader.GetDateTime(timeOrd)).ToUnixTimeSeconds()
            });
        }

        return games;
    }

    public static async Task<(CardData Card, DeckType DeckType)> GetCard(long cardId, long userId = 0, DeckType? searchDeckType = null)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        var sql = new StringBuilder(@"
        SELECT *, deck_type 
        FROM hut_cards
        WHERE card_id = @card_id");

        if (userId != 0) sql.Append(" AND user_id = @user_id");
        if (searchDeckType.HasValue) sql.Append(" AND deck_type = @search_deck_type");

        await using var cmd = new NpgsqlCommand(sql.ToString(), conn);
        cmd.Parameters.AddWithValue("card_id", cardId);
        if (userId != 0) cmd.Parameters.AddWithValue("user_id", userId);
        if (searchDeckType.HasValue) cmd.Parameters.AddWithValue("search_deck_type", (int)searchDeckType);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var card = HutHelper.ReadCardData(reader);
            DeckType deckType = (DeckType)reader.GetInt32(reader.GetOrdinal("deck_type"));

            return (card, deckType);
        }

        return (new CardData(), DeckType.CARDHOUSE_DECK_INVALID);
    }

    public static async Task<(CardData Card, DeckType DeckType)> GetCard(uint cardDbId, long userId = 0)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        var sql = new StringBuilder(@"
            SELECT * 
            FROM hut_cards
            WHERE db_id = @db_id");

        if (userId != 0) sql.Append(" AND user_id = @user_id");

        await using var cmd = new NpgsqlCommand(sql.ToString(), conn);
        cmd.Parameters.AddWithValue("db_id", (int)cardDbId);
        if (userId != 0) cmd.Parameters.AddWithValue("user_id", userId);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var card = HutHelper.ReadCardData(reader);
            DeckType deckType = (DeckType)reader.GetInt32(reader.GetOrdinal("deck_type"));

            return (card, deckType);
        }

        return (new CardData(), DeckType.CARDHOUSE_DECK_GENERAL);
    }

    public static async Task<int> RenameSquad(string name, long userId, int squadId)
    {
        const string query = @"
        UPDATE hut_squad_info
        SET squad_name = @name
        WHERE squad_id = @squad_id
        AND user_id = @user_id
        RETURNING squad_id";

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("name", name);
        cmd.Parameters.AddWithValue("squad_id", squadId);
        cmd.Parameters.AddWithValue("user_id", userId);

        return (int)await cmd.ExecuteScalarAsync();
    }

    public static async Task HardDeleteSquad(long userId, int squadId)
    {
        const string query = @"
        DELETE FROM hut_squad_info
        WHERE squad_id = @squad_id
        AND user_id = @user_id";

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("squad_id", squadId);
        cmd.Parameters.AddWithValue("user_id", userId);

        await cmd.ExecuteNonQueryAsync();
    }

    public static async Task<int> SaveSquadInfo(SquadSaveRequest request, long userId)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("user_id", userId);

        if (request.mCopyCurrent == 1)
        {
            cmd.CommandText = @"
        INSERT INTO hut_squad_info (
            user_id, chemistry, formation_id, lines, 
            manager, squad_name, players, rating_def, rating_gk, rating_off, star_rating, active
        )
        SELECT 
            user_id, chemistry, formation_id, lines,
            manager, @squad_name, players, rating_def, rating_gk, rating_off, star_rating,
            false
        FROM hut_squad_info
        WHERE user_id = @user_id
        AND active = true
        RETURNING squad_id";

            cmd.Parameters.AddWithValue("squad_name", request.mSquadName);
        }
        else
        {
            string squadIdValue = request.mSquadId == 0 ? "DEFAULT" : "@squad_id";

            cmd.CommandText = $@"
            INSERT INTO hut_squad_info (
                squad_id, user_id, chemistry, formation_id, lines, 
                manager, squad_name, players, rating_def, rating_gk, rating_off, star_rating, active
            )
            VALUES (
                {squadIdValue}, @user_id, @chemistry, @formation_id, @lines, 
                @manager, @squad_name, @players, @rating_def, @rating_gk, @rating_off, @star_rating,
                NOT EXISTS (SELECT 1 FROM hut_squad_info WHERE user_id = @user_id)
            )
            ON CONFLICT (squad_id) DO UPDATE SET
                chemistry    = EXCLUDED.chemistry,
                formation_id = EXCLUDED.formation_id,
                lines        = EXCLUDED.lines,
                manager      = EXCLUDED.manager,
                squad_name   = EXCLUDED.squad_name,
                players      = EXCLUDED.players,
                rating_def   = EXCLUDED.rating_def,
                rating_gk    = EXCLUDED.rating_gk,
                rating_off   = EXCLUDED.rating_off,
                star_rating  = EXCLUDED.star_rating
            WHERE hut_squad_info.user_id = @user_id
            RETURNING squad_id";

            if (request.mSquadId != null && request.mSquadId != 0) cmd.Parameters.AddWithValue("squad_id", request.mSquadId);
            cmd.Parameters.AddWithValue("chemistry", (int)request.mChemistry);
            cmd.Parameters.AddWithValue("formation_id", (int)request.mFormation);
            cmd.Parameters.AddWithValue("lines", request.mLines);
            cmd.Parameters.AddWithValue("manager", request.mManager);
            cmd.Parameters.AddWithValue("squad_name", request.mSquadName);
            cmd.Parameters.AddWithValue("players", request.mPlayers != null ? request.mPlayers : new List<long>(new long[31]));
            cmd.Parameters.AddWithValue("rating_def", (int)request.mRatingDefensive);
            cmd.Parameters.AddWithValue("rating_gk", (int)request.mRatingGoalies);
            cmd.Parameters.AddWithValue("rating_off", (int)request.mRatingOffensive);
            cmd.Parameters.AddWithValue("star_rating", (int)request.mStarRating);
        }

        await using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        return reader.GetInt32(reader.GetOrdinal("squad_id"));
    }

    public static async Task<Dictionary<int, int>> GetTeamCardCountsAsync(long userId, int leagueId, DeckType deckType, params CardSubType[] subTypes)
    {
        var counts = new Dictionary<int, int>();

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        string sql = @"
            SELECT h.team_id, COUNT(*) 
            FROM hut_cards h
            INNER JOIN fcc_leagues l ON h.team_id = l.teamid
            WHERE h.user_id = @user_id 
            AND l.leagueid = @league_id 
            AND h.deck_type = @deck_type";

        if (subTypes.Length > 0)
        {
            sql += " AND h.sub_type = ANY(@sub_types)";
        }

        sql += " GROUP BY h.team_id";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("league_id", leagueId);
        cmd.Parameters.AddWithValue("deck_type", (int)deckType);

        if (subTypes.Length > 0)
        {
            cmd.Parameters.AddWithValue("sub_types", subTypes.Select(s => (short)s).ToArray());
        }

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            counts[reader.GetInt32(0)] = (int)reader.GetInt64(1);
        }

        return counts;
    }

    public static async Task<int> GetCardCountAsync(long userId, DeckType deckType, byte? formationId = null, bool onlyLegends = false, params CardSubType[] subTypes)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        string sql = "SELECT COUNT(*) FROM hut_cards WHERE user_id = @user_id";

        sql += " AND deck_type = @deck_type";

        if (formationId.HasValue) sql += " AND formation_id = @formationId";
        if (onlyLegends) sql += " AND db_id > 100000000";

        if (subTypes.Length > 0)
        {
            sql += " AND sub_type = ANY(@sub_types)";
        }

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("deck_type", (int)deckType);
        if (formationId.HasValue) cmd.Parameters.AddWithValue("formationId", (short)formationId.Value);

        if (subTypes.Length > 0)
        {
            cmd.Parameters.AddWithValue("sub_types", subTypes.Select(s => (short)s).ToArray());
        }

        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public static async Task<List<CardIdPair>> FindDuplicates(long userId, List<CardData> newCards)
    {
        int[] dbIdsToCheck = newCards.Select(c => (int)c.mCardDbId).ToArray();
        var duplicates = new List<CardIdPair>();

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        string sql = @"
        SELECT card_id, db_id 
        FROM hut_cards 
        WHERE user_id = @user_id 
        AND db_id = ANY(@db_ids)
        AND deck_type = 7";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("db_ids", dbIdsToCheck);

        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            long existingCardId = reader.GetInt64(0);
            int foundDbId = reader.GetInt32(1);
            duplicates.Add(new CardIdPair
            {
                mCardId = newCards.FirstOrDefault(c => c.mCardDbId == foundDbId).mCardId,
                mDuplicateCardId = existingCardId
            });
        }

        return duplicates;
    }


    public static async Task<List<CardData>> GetCardList(long userId, StickerBookSearchRequest request)
    {
        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        var sql = new StringBuilder(@"
            SELECT h.* FROM hut_cards h
            INNER JOIN fcc_leagues l ON h.team_id = l.teamid
            WHERE h.user_id = @user_id 
            AND h.deck_type = @deck_type");

        var deckType = DeckType.CARDHOUSE_DECK_STICKERBOOK;

        sql.Append(" AND h.deck_type = @deck_type");
        switch (request.mCollectionSearchCardType)
        {
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_ALL: break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_HEADCOACH: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_STAFF_HEADCOACH); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_BADGE: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_BADGE); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_STADIUM: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_STADIUM); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER_C: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_C); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER_LW: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LW); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER_RW: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RW); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER_LD: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LD); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER_RD: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RD); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER_GK: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_GK); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER_ALL: sql.Append(" AND sub_type = ANY(@fieldPlayerTypes)"); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER: sql.Append(" AND sub_type = ANY(@playerTypes)"); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_DEVELOPMENT: sql.Append(" AND sub_type = ANY(@consumableTypes)"); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_OFFLINE_TROPHY: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_OFFLINE); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_ONLINE_TROPHY: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_ONLINE); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_LIVE_TROPHY: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_LIVE); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYOFF_TROPHY: sql.Append(" AND sub_type = " + (int)CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_PLAYOFF); break;
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER_STAR_OF_THE_WEEK: sql.Append(" AND sub_type = -1"); break; //Not implemented because this is a little ambiguous currently
            case CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER_LEGEND: sql.Append(" AND db_id > 100000000"); break;
            default: throw new NotImplementedException();
        }

        if (request.mCardState != null)
        {
            switch (request.mCardState)
            {
                case CardState.CARDHOUSE_CARDSTATE_SEARCH_ACTIVE: sql.Append(" AND state_id = ANY(@activeStates)"); break;
            }
        }

        if (request.mLeagueId >= 0) sql.Append(" AND l.leagueid = @league_id");
        if (request.mTeamId >= 0) sql.Append(" AND h.team_id = @team_id");

        sql.Append(" ORDER BY rating DESC");
        if (request.mNumRetrieve > 0) sql.Append(" LIMIT " + request.mNumRetrieve);
        if (request.mStart > 0) sql.Append(" OFFSET " + request.mStart);

        await using var cmd = new NpgsqlCommand(sql.ToString(), conn);
        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("deck_type", (int)deckType);

        if (request.mLeagueId >= 0) cmd.Parameters.AddWithValue("league_id", request.mLeagueId);
        if (request.mTeamId >= 0) cmd.Parameters.AddWithValue("team_id", request.mTeamId);
        if (request.mCollectionSearchCardType == CollectionSearchType.COLLECTION_SEARCH_TYPE_DEVELOPMENT) cmd.Parameters.AddWithValue("consumableTypes", CardHouseComponent.ConsumablesTypes.Select(x => (int)x).ToArray());
        if (request.mCollectionSearchCardType == CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER) cmd.Parameters.AddWithValue("playerTypes", CardHouseComponent.PlayerTypes.Select(x => (int)x).ToArray());
        if (request.mCollectionSearchCardType == CollectionSearchType.COLLECTION_SEARCH_TYPE_PLAYER_ALL) cmd.Parameters.AddWithValue("fieldPlayerTypes", CardHouseComponent.FieldPlayerTypes.Select(x => (int)x).ToArray());
        if (request.mCardState != null && request.mCardState == CardState.CARDHOUSE_CARDSTATE_SEARCH_ACTIVE) cmd.Parameters.AddWithValue("activeStates", CardHouseComponent.ActiveStates.Select(x => (int)x).ToArray());

        await using var reader = await cmd.ExecuteReaderAsync();

        List<CardData> cardDataList = new List<CardData>();

        while (await reader.ReadAsync())
        {
            cardDataList.Add(HutHelper.ReadCardData(reader));
        }

        return cardDataList;
    }

    public static async Task<bool> IsTeamNameAvailable(string teamName)
    {
        const string query = @"
            SELECT COUNT(*) 
            FROM hut_name_reservations
            WHERE LOWER(team_name) = LOWER(@teamName)
              AND set_free = false";

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("teamName", teamName);

        var count = (long)(await cmd.ExecuteScalarAsync())!;
        return count == 0;
    }

    public static async Task<bool> IsFirstTeam(long userId)
    {
        const string query = @"
            SELECT COUNT(*) 
            FROM hut_name_reservations
            WHERE user_id = @user_id";

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("user_id", userId);

        var count = (long)(await cmd.ExecuteScalarAsync())!;
        return count <= 1;
    }

    public static async Task InsertNameReservation(long userId, string userName, string teamName, string teamAbbreviation)
    {
        const string query = @"
            INSERT INTO hut_name_reservations (user_id, user_name, team_name, team_abbreviation)
            VALUES (@userId, @userName, @teamName, @teamAbbreviation)";

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("userId", userId);
        cmd.Parameters.AddWithValue("userName", userName);
        cmd.Parameters.AddWithValue("teamName", teamName);
        cmd.Parameters.AddWithValue("teamAbbreviation", teamAbbreviation);

        await cmd.ExecuteNonQueryAsync();
    }

    public static async Task MarkTeamNameAsDeleted(string teamName)
    {
        const string query = @"
        UPDATE hut_name_reservations
        SET deleted_at = CURRENT_TIMESTAMP
        WHERE LOWER(team_name) = LOWER(@teamName)
          AND deleted_at IS NULL";

        await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(query, conn);
        cmd.Parameters.AddWithValue("teamName", teamName);

        await cmd.ExecuteNonQueryAsync();
    }

    public static async Task<bool> HardDelete(long userId, long? cardId = null)
    {
        await using var connection = new NpgsqlConnection(UltimateDatabase.ConnectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            if (cardId.HasValue)
            {
                await DeleteCard(userId, cardId.Value, connection, transaction);
            }
            else
            {
                bool canDelete = await CanDeleteUser(userId, connection, transaction);
                if (!canDelete) return false;

                await DeleteUser(userId, connection, transaction);
            }

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static async Task DeleteCard(long userId, long cardId, NpgsqlConnection conn, NpgsqlTransaction tx)
    {
        const string deleteCard = "DELETE FROM hut_cards WHERE card_id = @cardId AND user_id = @userId";

        await using var cmd = new NpgsqlCommand(deleteCard, conn, tx);
        cmd.Parameters.AddWithValue("cardId", cardId);
        cmd.Parameters.AddWithValue("userId", userId);
        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task<bool> CanDeleteUser(long userId, NpgsqlConnection conn, NpgsqlTransaction tx)
    {
        const string activeTradeQuery = @"
        SELECT COUNT(*) FROM hut_trade_info
        WHERE user_id = @userId
          AND trade_state = 1";

        await using (var cmd = new NpgsqlCommand(activeTradeQuery, conn, tx))
        {
            cmd.Parameters.AddWithValue("userId", userId);
            var count = (long)(await cmd.ExecuteScalarAsync())!;
            if (count > 0) return false;
        }

        const string activeOfferQuery = @"
        SELECT COUNT(*) FROM hut_offer_info
        WHERE user_id = @userId
          AND (offer_state = 1 OR offer_state = 7)";

        await using (var cmd = new NpgsqlCommand(activeOfferQuery, conn, tx))
        {
            cmd.Parameters.AddWithValue("userId", userId);
            var count = (long)(await cmd.ExecuteScalarAsync())!;
            if (count > 0) return false;
        }

        return true;
    }

    private static async Task DeleteUser(long userId, NpgsqlConnection conn, NpgsqlTransaction tx)
    {
        string? teamName = await GetTeamName(userId, conn, tx);

        string[] tables =
        [
            "hut_watching",
            "hut_offer_info",
            "hut_trade_info",
            "hut_cards",
            "hut_squad_info",
            "hut_gamer_info",
            "hut_version_info",
            "hut_general_info",
            "hut_tournaments"
        ];

        foreach (var table in tables)
        {
            var query = $"DELETE FROM {table} WHERE user_id = @userId";
            await using var cmd = new NpgsqlCommand(query, conn, tx);
            cmd.Parameters.AddWithValue("userId", userId);
            await cmd.ExecuteNonQueryAsync();
        }

        if (teamName != null) await MarkTeamNameAsDeleted(teamName);
    }

    private static async Task<string?> GetTeamName(long userId, NpgsqlConnection conn, NpgsqlTransaction tx)
    {
        const string query = "SELECT team_name FROM hut_gamer_info WHERE user_id = @userId";
        await using var cmd = new NpgsqlCommand(query, conn, tx);
        cmd.Parameters.AddWithValue("userId", userId);
        return (string?)await cmd.ExecuteScalarAsync();
    }
}