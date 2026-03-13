using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.StockRequestModels
{


    public class StockRequest
    {
        public long Id { get; set; }
        public StockRequestType RequestType { get; set; }
        public StockRequestStatus Status { get; set; }
        public PurchasePriority Priority { get; set; }
        public long? ProjectId { get; set; }
        public long RequestedByUserId { get; set; }
        public DateTimeOffset RequestedAt { get; set; }

        public long? ApprovedByUserId { get; set; }
        public DateTimeOffset? ApprovedAt { get; set; }
        public string? ApprovalNote { get; set; }

        public string? Note { get; set; }

        public long? ReceivedConfirmedByUserId { get; set; }
        public DateTimeOffset? ReceivedConfirmedAt { get; set; }
        public string? ReceivedConfirmationNote { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Project? Project { get; set; }
        public User RequestedByUser { get; set; } = null!;
        public User? ApprovedByUser { get; set; }
        public User? ReceivedConfirmedByUser { get; set; }
        public List<StockRequestItem> Items { get; set; } = new();
        public List<StockRequestHistory> History { get; set; } = new();

    }

}
