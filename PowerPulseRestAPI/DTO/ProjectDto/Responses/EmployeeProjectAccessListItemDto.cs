using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class EmployeeProjectAccessListItemDto
    {
        public long Id { get; set; }

        [Required]
        public long ProjectId { get; set; }

        [Required, MaxLength(50)]
        public string ProjectCode { get; set; } = null!;

        [Required, MaxLength(200)]
        public string ProjectName { get; set; } = null!;

        public bool IsEnabled { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
