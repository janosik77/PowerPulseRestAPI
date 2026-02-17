using PowerPulseRestAPI.Data.Models.ToolsModels;

namespace PowerPulseRestAPI.Data.Models.StockRequestModels
{


    public class StorageLocation
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<MaterialStock> MaterialStocks { get; set; } = new();
        public List<MaterialMovement> MovementsFrom { get; set; } = new();
        public List<MaterialMovement> MovementsTo { get; set; } = new();
        public List<ToolAssetStock> ToolAssetStocks { get; set; } = new();
        public List<ToolAssignment> ToolAssignmentsToHere { get; set; } = new();
        public List<StockRequestReservation> StockRequestReservations { get; set; } = new();
        public List<StockRequestFulfillmentItem> StockRequestFulfillmentItemsFromHere { get; set; } = new();
        public List<StockRequestReservation> StockRequestReservationsHere { get; set; } = new();


    }

}
