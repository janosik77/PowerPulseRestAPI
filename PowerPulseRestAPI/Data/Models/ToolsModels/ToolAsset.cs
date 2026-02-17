using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.StockRequestModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.ToolsModels
{


    public class ToolAsset
    {
        public long Id { get; set; }
        public long ToolId { get; set; }
        public string? SerialNumber { get; set; }
        public string? AssetTag { get; set; }
        public ToolCondition Condition { get; set; }
        public ToolAssetStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Tool? Tool { get; set; }
        public ToolAssetStock? Stock { get; set; }          // 1:1
        public List<ToolAssignment> Assignments { get; set; } = new();
        public List<ToolIssue> Issues { get; set; } = new();
        public List<StockRequestReservation> StockRequestReservations { get; set; } = new();
        public List<StockRequestFulfillmentItem> StockRequestFulfillmentItems { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();

    }

}
