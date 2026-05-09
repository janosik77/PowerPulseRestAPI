using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectTaskListItemDto
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? Caption { get; set; }
        public string Priority { get; set; } = null!;    
        public ProjectTaskStatus Status { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        public int? EstimatedHours { get; set; }
        public long CreatedByEmployeeId { get; set; }
        public string CreatedByEmployeeName { get; set; } = null!;
        public long? AssignedEmployeeId { get; set; }
        public string? AssignedEmployeeName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

    }
}
