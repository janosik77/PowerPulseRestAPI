using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectListItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
    }
}
