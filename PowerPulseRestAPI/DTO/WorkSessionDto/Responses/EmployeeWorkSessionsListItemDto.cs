using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.WorkSessionDto.Responses
{
    public class EmployeeWorkSessionsListItemDto
    {
        public long Id { get; set; }

        [Required]
        public long ProjectId { get; set; }

        [Required, MaxLength(50)]
        public string ProjectCode { get; set; } = null!;

        [Required, MaxLength(200)]
        public string ProjectName { get; set; } = null!;

        [Required]
        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset? EndedAt { get; set; }

        [Required]
        public WorkSessionStatus Status { get; set; }

    }
}
