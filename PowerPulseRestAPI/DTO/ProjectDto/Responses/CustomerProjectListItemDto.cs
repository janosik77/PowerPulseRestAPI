using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class CustomerProjectListItemDto
    {
        public long Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        public ProjectStatus Status { get; set; }

    }
}
