using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.StockRequestModels
{


    public class StockRequestReservation
    {
        public long Id { get; set; }
        public long StockRequestItemId { get; set; }
        public StockItemType ItemType { get; set; }
        public long? MaterialId { get; set; }
        public decimal? ReservedQuantity { get; set; }
        public long? StorageLocationId { get; set; }
        public long? ToolAssetId { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTimeOffset ReservedAt { get; set; }
        public long ReservedByUserId { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public StockRequestItem? StockRequestItem { get; set; }
        public Material? Material { get; set; }
        public StorageLocation? StorageLocation { get; set; }
        public ToolAsset? ToolAsset { get; set; }
        public User? ReservedByUser { get; set; }
    }

}
