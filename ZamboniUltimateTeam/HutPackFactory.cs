using NLog;
using Npgsql;
using ZamboniUltimateTeam.Config;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam
{
    public static class HutPackFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<string, (string Table, CardSubType? SubType)> CardTypeTableMap = new()
        {
            { "Badge", ("fcc_badges", CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_BADGE) },
            { "Contract", ("fcc_contractcards", CardSubType.CARDHOUSE_CARD_TYPE_CONTRACT_PLAYER) },
            { "Kit", ("fcc_kitcards", null) },
            { "Player", ("fcc_playercards", null) },
            { "Stadium", ("fcc_stadium", CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_STADIUM) },
            { "Training", ("fcc_trainingcards", null) },
            { "Coach", ("fcc_headcoachcards", CardSubType.CARDHOUSE_CARD_TYPE_STAFF_HEADCOACH) },
            { "Healing", ("fcc_healingcards", null) },
        };

        public static async Task<List<CardData>> RollPackAsync(Pack pack, long userId)
        {
            var results = new List<CardData>();
            var categoryCounts = new Dictionary<string, int>();

            foreach (var cardSpec in pack.Loot.Guaranteed)
            {
                for (int i = 0; i < cardSpec.Count; i++)
                {
                    results.Add(await RollCard(userId, cardSpec.CardType, cardSpec.Filters, results));
                }
            }

            foreach (var randomizedCategory in pack.Loot.Randomized)
            {
                for (int i = 0; i < randomizedCategory.Count; i++)
                {
                    CardSpec item;
                    do
                    {
                        item = PickWeightedItem(randomizedCategory);
                    } while (pack.Loot.Limits.TryGetValue(item.CardType, out int limit) && categoryCounts.GetValueOrDefault(item.CardType) >= limit);

                    var result = await RollCard(userId, item.CardType, item.Filters, results);
                    categoryCounts[item.CardType] = categoryCounts.GetValueOrDefault(item.CardType) + 1;
                    results.Add(result);
                }
            }

            results.Sort((_, _) => Random.Shared.Next(-1, 2));
            return results;
        }

        private static CardSpec PickWeightedItem(RandomizedCategory pool)
        {
            float roll = (float)(Random.Shared.NextDouble() * pool.Weights.Values.Sum());

            foreach (var item in pool.Items)
            {
                roll -= pool.Weights[item.Category!];
                if (roll < 0)
                    return item;
            }

            return pool.Items.Last();
        }

        private static async Task<CardData> RollCard(long userId, string cardType, Filters? filters, List<CardData> alreadyRolled)
        {
            if (!CardTypeTableMap.TryGetValue(cardType, out var table)) throw new Exception($"Card type: {cardType} not mapped to a table");

            var query = BuildQuery(table.Table, filters);

            uint dbId;
            do
            {
                await using var conn = new NpgsqlConnection(UltimateDatabase.ConnectionString);
                await conn.OpenAsync();
                await using var cmd = new NpgsqlCommand(query, conn);
                var result = await cmd.ExecuteScalarAsync();
                if (result is null) throw new Exception("Roll returned 0 available matches");
                dbId = Convert.ToUInt32(result);
            } while (!CanHaveDuplicatesInSamePack(cardType) && alreadyRolled.Any(card => card.mCardDbId == dbId));

            if (cardType.Equals("Player"))
            {
                return await HutCardFactory.CreatePlayerCard(userId, dbId);
            }

            CardSubType? subType = CardTypeTableMap[cardType].SubType;
            if (subType.HasValue)
            {
                return await HutCardFactory.CreateNonPlayerCard(userId, dbId, subType.Value);
            }

            if (cardType.Equals("Healing"))
            {
                var healingCard = await UltimateDatabase.GetHealingCardByDbIdAsync(dbId);
                return await HutCardFactory.CreateNonPlayerCard(userId, dbId, (CardSubType)healingCard.CardSubType);
            }

            if (cardType.Equals("Training"))
            {
                var trainingCard = await UltimateDatabase.GetTrainingCardByDbIdAsync(dbId);
                return await HutCardFactory.CreateNonPlayerCard(userId, dbId, (CardSubType)trainingCard.CardSubType);
            }

            if (cardType.Equals("Kit"))
            {
                var kitCard = await UltimateDatabase.GetKitCardByDbIdAsync(dbId);
                return await HutCardFactory.CreateNonPlayerCard(userId, dbId, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_KIT, (byte)(kitCard.IsAway ? 1 : 0));
            }

            throw new Exception();
        }

        private static bool CanHaveDuplicatesInSamePack(string cardType)
        {
            return cardType.Equals("Contract") || cardType.Equals("Healing") || cardType.Equals("Training");
        }

        private static string BuildQuery(string table, Filters? filters)
        {
            var clauses = new List<string>();

            if (filters != null)
            {
                if (filters.Rating != null) clauses.Add($"rating >= {filters.Rating.RangeStart} AND rating <= {filters.Rating.RangeEnd}");
                if (filters.ZRating != null) clauses.Add($"zrating >= {filters.ZRating.RangeStart} AND zrating <= {filters.ZRating.RangeEnd}");
                if (filters.Zcat != null) clauses.Add($"zcat >= {filters.Zcat.RangeStart} AND zcat <= {filters.Zcat.RangeEnd}");
                if (filters.Rare.HasValue) clauses.Add($"rare = {filters.Rare.Value}");
                if (filters.ZVictory.HasValue) clauses.Add($"zvictory = {filters.ZVictory.Value}");
                if (filters.ZLegendary.HasValue) clauses.Add($"zlegendary = {filters.ZLegendary.Value}");
                if (filters.IsAway.HasValue) clauses.Add($"isaway = {filters.IsAway.Value}");
                if (filters.Alternative.HasValue) clauses.Add($"alternative = {filters.Alternative.Value}");
                if (filters.PreferredPosition.HasValue) clauses.Add($"preferredposition = {filters.PreferredPosition.Value}");
            }

            var where = clauses.Count > 0 ? "WHERE " + string.Join(" AND ", clauses) : "";
            var query = $"SELECT carddbid FROM {table} {where} ORDER BY RANDOM() LIMIT 1";
            Logger.Debug("RollQuery " + query);
            return query;
        }
    }
}