using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ToolDto.Responses
{
    public class ToolAssignmentDto
    {
        public long Id { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string EmployeeName { get; set; } = null!;

        [Required]
        public DateTimeOffset AssignedAt { get; set; }

        public DateTimeOffset? ReturnedAt { get; set; }

        [MaxLength(2000)]
        public string? Notes { get; set; }
    }
}
