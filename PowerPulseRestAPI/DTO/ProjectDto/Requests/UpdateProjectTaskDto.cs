using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;


namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class UpdateProjectTaskDto
    {
        [MaxLength(300)]
        public string? Title { get; set; }
        [MaxLength(4000)]
        public string? Description { get; set; }
        [MaxLength(2048)]
        public string? Url { get; set; }
        [MaxLength(500)]
        public string? Caption { get; set; }
        [MaxLength(30)]
        public string? Priority { get; set; }
        public ProjectTaskStatus? Status { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        [Range(0, int.MaxValue)]
        public int? EstimatedHours { get; set; }
        public long? AssignedToEmployeeId { get; set; }

    }
}
