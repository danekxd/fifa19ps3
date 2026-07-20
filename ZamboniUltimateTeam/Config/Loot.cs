namespace ZamboniUltimateTeam.Config
{
    public class Loot
    {
        public List<CardSpec> Guaranteed { get; set; } = new();
        public List<RandomizedCategory> Randomized { get; set; } = new();
        public Dictionary<string, int> Limits { get; set; } = new();

    }

    public class CardSpec
    {
        public string CardType { get; set; }
        public int Count { get; set; } = 1;
        public string? Category { get; set; }
        public Filters? Filters { get; set; }
    }

    public class RandomizedCategory
    {
        public int Count { get; set; }
        public Dictionary<string, float> Weights { get; set; } = new();
        public List<CardSpec> Items { get; set; } = new();
    }

    public class Range
    {
        public int RangeStart { get; set; }
        public int RangeEnd { get; set; }
    }

    public class Filters
    {
        public Range? Rating { get; set; }
        public Range? ZRating { get; set; }
        public Range? Zcat { get; set; }
        public int? Rare { get; set; }
        public int? ZVictory { get; set; }
        public int? ZLegendary { get; set; }
        public bool? IsAway { get; set; }
        public bool? Alternative { get; set; }
        public int? PreferredPosition { get; set; }
    }
    
}