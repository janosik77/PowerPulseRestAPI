using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.PurchaseReqModels
{


    public class PurchaseRequest
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long RequestedByUserId { get; set; }
        public PurchasePriority Priority { get; set; }
        public PurchaseStatus Status { get; set; }
        public string? Note { get; set; }
        public long? ApprovedByUserId { get; set; }
        public DateTimeOffset? ApprovedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Project? Project { get; set; }
        public User? RequestedByUser { get; set; }
        public User? ApprovedByUser { get; set; }
        public List<PurchaseRequestItem> Items { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();

    }

}
