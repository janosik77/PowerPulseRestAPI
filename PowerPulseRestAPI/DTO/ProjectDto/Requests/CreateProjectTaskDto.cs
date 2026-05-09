using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class CreateProjectTaskDto
    {
        [Required]
        public long ProjectId { get; set; }
        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = null!;
        [MaxLength(4000)]
        public string? Description { get; set; }
        [MaxLength(2048)]
        public string? Url { get; set; }
        [MaxLength(500)]
        public string? Caption { get; set; }
        [Required]
        [MaxLength(30)]
        public string Priority { get; set; } = null!;
        [Required]
        public ProjectTaskStatus Status { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        [Range(0, int.MaxValue)]
        public int? EstimatedHours { get; set; }
        public long? AssignedToEmployeeId { get; set; }

    }
}
