using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.CustomerDto.Responses;

public class RecentCustomerDto
{
    public long Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? ContactPersonFullName { get; set; }
    public CustomerStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}