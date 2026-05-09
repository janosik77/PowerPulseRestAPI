using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ToolDto.Requests
{
    public class AssignToolDto
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }

        [MaxLength(2000)]
        public string? Notes { get; set; }
    }
}
