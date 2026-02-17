using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.UsersModels
{


    public class WorkSessionEvent
    {
        public long Id { get; set; }
        public long WorkSessionId { get; set; }
        public WorkSessionEventType EventType { get; set; }
        public DateTimeOffset EventAt { get; set; }
        public string? Note { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public WorkSession? WorkSession { get; set; }
        public User? CreatedByUser { get; set; }
    }

}
