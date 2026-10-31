using PowerPulseRestAPI.Reports.Models;

namespace PowerPulseRestAPI.Reports;

public interface IAnalyticsReportService
{
    Task<GeneratedReport> GenerateAsync(
        AnalyticsReportRequest request,
        string? generatedBy,
        CancellationToken cancellationToken = default
    );
}