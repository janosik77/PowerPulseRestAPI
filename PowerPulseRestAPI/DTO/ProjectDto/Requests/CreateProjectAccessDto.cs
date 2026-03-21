namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class CreateProjectAccessDto
    {
        public long ProjectId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
