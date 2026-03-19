using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectTaskListDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Url { get; set; } = null!;
        public string? Caption { get; set; }
        public TaskPriority Priority { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        public int? EstimatedHours { get; set; }
        public long CreatedByUserName { get; set; }
        public long? AssignedEmployeeName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

    }
}
