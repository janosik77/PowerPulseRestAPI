using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class EmployeeTaskListItemDto
    {
        public long Id { get; set; }

        public string Title { get; set; } = null!;

        public string projectName { get; set; } = null!;

        public string Priority { get; set; } = null!;

        public ProjectTaskStatus Status { get; set; }

    }
}
