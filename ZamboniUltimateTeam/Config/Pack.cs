using YamlDotNet.Serialization;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Config
{
    public class Pack
    {
        public int PackId { get; set; }
        public int PackStoreId { get; set; }
        public int CoinCost { get; set; }
        public int Attributes { get; set; }
        public int Availability { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public int Quantity { get; set; }
        public int SaleType { get; set; }
        public int State { get; set; }

        public Loot Loot { get; set; } = new();

        [YamlIgnore] 
        public PackType PackType => (PackType)PackId;
        
        [YamlIgnore]
        public StorePackTypeData StorePackTypeData => new()
        {
            mId = (StorePackId)PackStoreId,
            mCoinCost = CoinCost,
            mAttributes = (StorePackAttribute)Attributes,
            mAvailability = (StorePackAvailability)Availability,
            mStartDate = (uint)StartDate,
            mEndDate = (uint)EndDate,
            mQuantity = Quantity,
            mSaleType = (StoreSaleType)SaleType,
            mState = (StorePackState)State,
        };
    }
}