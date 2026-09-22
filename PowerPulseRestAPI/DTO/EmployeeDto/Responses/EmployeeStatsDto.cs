namespace PowerPulseRestAPI.DTO.EmployeeDto.Responses;

public class EmployeeStatsDto
{
    public EmployeeStatsSummaryDto Summary { get; set; } = new();
    public List<RecentEmployeeStatsItemDto> RecentEmployees { get; set; } = new();
}