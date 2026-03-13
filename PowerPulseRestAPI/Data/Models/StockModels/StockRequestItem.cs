using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.MaterialsModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;

namespace PowerPulseRestAPI.Data.Models.StockRequestModels
{


    public class StockRequestItem
    {
        public long Id { get; set; }
        public long StockRequestId { get; set; }
        public StockItemType ItemType { get; set; }
        public long? MaterialId { get; set; }
        public decimal? RequestedQuantity { get; set; }
        public decimal? FulfilledQuantity { get; set; }
        public decimal? ReceivedQuantity { get; set; }
        public string? Unit { get; set; }
        public long? ToolId { get; set; }
        public int? RequestedCount { get; set; }
        public int? FulfilledCount { get; set; }
        public int? ReceivedCount { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public StockRequest StockRequest { get; set; } = null!;
        public Material? Material { get; set; }
        public Tool? Tool { get; set; }

    }

}
