namespace PowerPulseRestAPI.DTO.StatsDto.CustomerStats.Responses;

public class CustomerStatsSummaryDto
{
    public int TotalCustomers { get; set; }
    public int ActiveCustomers { get; set; }
    public int NewThisMonth { get; set; }
    public int CustomersWithProjects { get; set; }
}