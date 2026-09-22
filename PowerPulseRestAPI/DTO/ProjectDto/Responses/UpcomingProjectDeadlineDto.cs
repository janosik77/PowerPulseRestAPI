using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Responses;

public class UpcomingProjectDeadlineDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public string? CustomerName { get; set; }

    public ProjectStatus Status { get; set; }

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public int DaysLeft { get; set; }
}