namespace PowerPulseRestAPI.DTO.ProjectDto.Responses;

public class ProjectStatsSummaryDto
{
    public int TotalProjects { get; set; }
    public int ActiveProjects { get; set; }
    public int CompletedProjects { get; set; }
    public int OverdueProjects { get; set; }
}