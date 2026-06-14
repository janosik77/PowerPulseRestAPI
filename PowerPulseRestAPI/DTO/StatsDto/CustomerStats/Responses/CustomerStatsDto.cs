namespace PowerPulseRestAPI.DTO.StatsDto.CustomerStats.Responses;

public class CustomerStatsDto
{
    public CustomerStatsSummaryDto Summary { get; set; } = new();
    public List<RecentCustomerDto> RecentCustomers { get; set; } = new();
}