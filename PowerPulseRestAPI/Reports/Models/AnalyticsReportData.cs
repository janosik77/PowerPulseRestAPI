using PowerPulseRestAPI.DTO.AnalyticsDto.Responses;

namespace PowerPulseRestAPI.Reports.Models;

public class AnalyticsReportData
{
    public string Title { get; set; } = "Analysis Report";
    
    public string? Subtitle { get; set; }
    
    public DateTimeOffset GeneratedAt { get; set; } = DateTimeOffset.Now;
    
    public string GeneratedBy { get; set; } = "-";

    public string ProjectLabel { get; set; } = "All projects";
    
    public string EmployeeLabel { get; set; } = "All employees";
    
    public string CustomerLabel { get; set; } = "All customers";

    public AnalyticsOverviewDto Analytics { get; set; } = new();
}