using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ToolsModels;

namespace PowerPulseRestAPI.Data.Models.PurchaseReqModels
{


    public class PurchaseRequestItem
    {
        public long Id { get; set; }
        public long PurchaseRequestId { get; set; }
        public StockItemType ItemType { get; set; } // MATERIAL / TOOL
        public long? MaterialId { get; set; }
        public long? ToolId { get; set; }
        public string? ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public PurchaseRequest? PurchaseRequest { get; set; }
        public Material? Material { get; set; }
        public Tool? Tool { get; set; }
    }

}
