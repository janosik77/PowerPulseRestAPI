namespace PowerPulseRestAPI.DTO.ProjectDto.Responses;

public class ProjectStatsDto
{
    public ProjectStatsSummaryDto Summary { get; set; } = new();
    public List<UpcomingProjectDeadlineDto> UpcomingDeadlines { get; set; } = new();
}