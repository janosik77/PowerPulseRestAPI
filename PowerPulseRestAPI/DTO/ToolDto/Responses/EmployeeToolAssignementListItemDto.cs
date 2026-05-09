using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ToolDto.Responses
{
    public class EmployeeToolAssignementListItemDto
    {
        public long Id { get; set; }

        [Required]
        public long ToolId { get; set; }

        [Required, MaxLength(200)]
        public string ToolName { get; set; } = null!;

        [Required]
        public DateTimeOffset AssignedAt { get; set; }

        public DateTimeOffset? ReturnedAt { get; set; }
    }
}
