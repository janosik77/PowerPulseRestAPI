using PowerPulseRestAPI.Data.Models.PurchaseReqModels;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

namespace PowerPulseRestAPI.Data.Models.ToolsModels
{


    public class Tool
    {
        public long Id { get; set; }
        public string? Sku { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long CategoryId { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? Barcode { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public ToolCategory? Category { get; set; }
        public List<ToolImage> Images { get; set; } = new();
        public List<ToolAsset> Assets { get; set; } = new();
        public List<PurchaseRequestItem> PurchaseRequestItems { get; set; } = new();
        public List<StockRequestItem> StockRequestItems { get; set; } = new();
    }

}
