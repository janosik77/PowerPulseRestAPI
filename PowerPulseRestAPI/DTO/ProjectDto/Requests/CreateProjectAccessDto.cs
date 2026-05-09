using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class CreateProjectAccessDto
    {
        [Required]
        public long ProjectId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
