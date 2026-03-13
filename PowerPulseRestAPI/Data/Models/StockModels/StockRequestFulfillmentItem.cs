using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ToolsModels;

namespace PowerPulseRestAPI.Data.Models.StockRequestModels
{


    public class StockRequestFulfillmentItem
    {
        public long Id { get; set; }
        public long FulfillmentId { get; set; }
        public StockItemType ItemType { get; set; }
        public long? MaterialId { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public long? FromStorageLocationId { get; set; }
        public long? ToolAssetId { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public StockRequestFulfillment? Fulfillment { get; set; }
        public Material? Material { get; set; }
        public StorageLocation? FromStorageLocation { get; set; }
        public ToolAsset? ToolAsset { get; set; }
    }

}
