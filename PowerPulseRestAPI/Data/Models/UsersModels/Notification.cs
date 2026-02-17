using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.PurchaseReqModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;

namespace PowerPulseRestAPI.Data.Models.UsersModels
{


    public class Notification
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public NotificationSeverity Severity { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset? ReadAt { get; set; }
        public long? RelatedProjectId { get; set; }
        public long? RelatedTaskId { get; set; }
        public long? RelatedPurchaseRequestId { get; set; }
        public long? RelatedVehicleId { get; set; }
        public long? RelatedToolAssetId { get; set; }
        public long? CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public User? User { get; set; }
        public Project? RelatedProject { get; set; }
        public ProjectTask? RelatedTask { get; set; }
        public PurchaseRequest? RelatedPurchaseRequest { get; set; }
        public Vehicle? RelatedVehicle { get; set; }
        public ToolAsset? RelatedToolAsset { get; set; }
        public User? CreatedByUser { get; set; }
    }

}
