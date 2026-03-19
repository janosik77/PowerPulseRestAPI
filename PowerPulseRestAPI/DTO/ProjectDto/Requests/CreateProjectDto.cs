using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class CreateProjectDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? AvatarUrl { get; set; }
        public string Code { get; set; } = null!;
        public ProjectStatus Status { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public long CreatedByUserId { get; set; }
        public long CustomerId { get; set; }
    }
}
