namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectAccessListItemDto
    {
        public long Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public bool IsEnabled { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
