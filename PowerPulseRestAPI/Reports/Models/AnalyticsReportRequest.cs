using PowerPulseRestAPI.DTO.AnalyticsDto.Requests;

namespace PowerPulseRestAPI.Reports.Models;

public class AnalyticsReportRequest
{
    public string Format { get; set; } = "pdf";
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public bool IncludeCharts { get; set; } = true;
    public AnalyticsFilterDto Filters { get; set; } = new();
    public AnalyticsReportSections Sections { get; set; } = new();
}

public class AnalyticsReportSections
{
    public bool Summary { get; set; } = true;
    public bool WorkHours { get; set; } = true;
    public bool Projects { get; set; } = true;
    public bool Materials { get; set; } = true;
    public bool Invoices { get; set; } = true;
    public bool Employees { get; set; } = true;

    public bool AnySelected()
    {
        return Summary || WorkHours || Projects || Materials || Invoices || Employees;
    }
}