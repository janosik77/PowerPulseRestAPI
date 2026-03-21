namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectAccessListItemDto
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
