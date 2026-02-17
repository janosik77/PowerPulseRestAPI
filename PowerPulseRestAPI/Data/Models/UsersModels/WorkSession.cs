using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Models.UsersModels
{


    public class WorkSession
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long ProjectId { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
        public WorkSessionStatus Status { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public long StartedByUserId { get; set; }

        public Employee? Employee { get; set; }
        public Project? Project { get; set; }
        public User? StartedByUser { get; set; }
        public List<WorkSessionEvent> Events { get; set; } = new();
    }

}
