using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class ProjectTask
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Url { get; set; } = null!;
        public string? Caption { get; set; }
        public string Priority { get; set; } = null!;
        public ProjectTaskStatus Status { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        public int? EstimatedHours { get; set; }
        public long CreatedByEmployeeId { get; set; }
        public long? AssignedToEmployeeId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Project Project { get; set; } = null!;
        public Employee CreatedByEmployee { get; set; } = null!;
        public Employee? AssignedToEmployee { get; set; }

    }

}
