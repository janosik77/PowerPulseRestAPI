namespace PowerPulseRestAPI.DTO.StatsDto.EmployeeStats.Responses;

public class EmployeeStatsDto
{
    public EmployeeStatsSummaryDto Summary { get; set; } = new();
    public List<RecentEmployeeStatsItemDto> RecentEmployees { get; set; } = new();
}