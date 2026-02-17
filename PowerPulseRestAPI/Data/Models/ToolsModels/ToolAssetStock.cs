using PowerPulseRestAPI.Data.Models.StockRequestModels;

namespace PowerPulseRestAPI.Data.Models.ToolsModels
{


    public class ToolAssetStock
    {
        public long ToolAssetId { get; set; }
        public long? StorageLocationId { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public ToolAsset? ToolAsset { get; set; }
        public StorageLocation? StorageLocation { get; set; }
    }

}
