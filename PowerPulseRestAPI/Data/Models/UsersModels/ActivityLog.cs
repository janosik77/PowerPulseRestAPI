using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.UsersModels
{
    public class ActivityLog
    {
        public long Id { get; set; }
        public ActivityEntityType EntityType { get; set; }
        public long EntityId { get; set; }
        public ActivityActionType ActionType { get; set; }
        public string Description { get; set; } = null!;
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public User? CreatedByUser { get; set; }
    }
}
