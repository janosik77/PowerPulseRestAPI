using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.WorkSessionDto.Requests
{
    public class CreateWorkSessionDto
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long ProjectId { get; set; }

        [Required]
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }

        [MaxLength(2000)]
        public string? Note { get; set; }
    }
}
