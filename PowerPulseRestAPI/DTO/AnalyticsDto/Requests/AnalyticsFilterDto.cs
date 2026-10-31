namespace PowerPulseRestAPI.DTO.AnalyticsDto.Requests;

public class AnalyticsFilterDto
{
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public long? ProjectId { get; set; }
    public long? EmployeeId { get; set; }
    public long? CustomerId { get; set; }
    public string? Currency { get; set; }
}