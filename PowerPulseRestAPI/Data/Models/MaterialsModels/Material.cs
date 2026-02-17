using PowerPulseRestAPI.Data.Models.PurchaseReqModels;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

namespace PowerPulseRestAPI.Data.Models
{
    
    
        public class Material
        {
            public long Id { get; set; }
            public string? Sku { get; set; }
            public string Name { get; set; } = null!;
            public string? Description { get; set; }
            public long CategoryId { get; set; }
            public string? Manufacturer { get; set; }
            public string? Barcode { get; set; }
            public string DefaultUnit { get; set; } = "pcs";
            public bool IsActive { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }

            public MaterialCategory? Category { get; set; }
            public List<MaterialImage> Images { get; set; } = new();
            public List<MaterialStock> Stocks { get; set; } = new();
            public List<MaterialMovement> Movements { get; set; } = new();
            public List<MaterialProjectBalance> ProjectBalances { get; set; } = new();
            public List<MaterialVehicleBalance> VehicleBalances { get; set; } = new();
            public List<PurchaseRequestItem> PurchaseRequestItems { get; set; } = new();
            public List<StockRequestItem> StockRequestItems { get; set; } = new();
            public List<StockRequestFulfillmentItem> StockRequestFulfillmentItems { get; set; } = new();
            public List<StockRequestReservation> StockRequestReservations { get; set; } = new();
    }

}
