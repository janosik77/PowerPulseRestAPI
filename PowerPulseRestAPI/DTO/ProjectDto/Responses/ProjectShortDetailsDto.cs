using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectShortDetailsDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public string Address { get; set; } = null!;
        public ProjectStatus Status { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string CustomerName { get; set; } = null!;
        public long? ResponsibleEmployeeId { get; set; }

    }
}
