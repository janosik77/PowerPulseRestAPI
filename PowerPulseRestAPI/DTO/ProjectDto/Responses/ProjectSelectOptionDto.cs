namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectSelectOptionDto
    {
        public string ProjectName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public long ProjectId { get; set; }
        public string Address { get; set; } = null!;
    }
}
