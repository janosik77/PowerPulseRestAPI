using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.StatsDto.EmployeeStats.Responses;

public class RecentEmployeeStatsItemDto
{
    public long Id { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string? JobTitle { get; set; }
    public string? Department { get; set; }

    public EmployeeStatus Status { get; set; }

    public DateOnly? HireDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}