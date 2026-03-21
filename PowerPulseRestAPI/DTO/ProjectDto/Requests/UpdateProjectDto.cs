using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class UpdateProjectDto
    {
        public long CustomerId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public long? ResponsibleEmployeeId { get; set; }
    }
}
