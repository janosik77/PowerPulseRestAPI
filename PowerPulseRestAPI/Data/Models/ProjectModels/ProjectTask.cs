using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class ProjectTask
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        public int? EstimatedMinutes { get; set; }
        public long CreatedByUserId { get; set; }
        public long? AssignedToEmployeeId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Project? Project { get; set; }
        public User? CreatedByUser { get; set; }
        public Employee? AssignedToEmployee { get; set; }
        public List<TaskUpdate> Updates { get; set; } = new();
        public List<TaskAttachment> Attachments { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();

    }

}
