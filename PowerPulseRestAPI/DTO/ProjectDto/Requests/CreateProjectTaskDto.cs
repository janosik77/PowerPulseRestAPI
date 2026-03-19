using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class CreateProjectTaskDto
    {
        public long ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Url { get; set; } = null!;
        public string? Caption { get; set; }
        public TaskPriority Priority { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        public int? EstimatedHours { get; set; }
        public long? AssignedToEmployeeId { get; set; }

    }
}
