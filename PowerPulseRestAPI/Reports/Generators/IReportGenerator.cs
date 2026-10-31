using PowerPulseRestAPI.Reports.Models;

namespace PowerPulseRestAPI.Reports.Generators;

public interface IReportGenerator
{
    string Format { get; }
    string ContentType { get; }
    string Extension { get; }

    byte[] Generate(
        AnalyticsReportData data,
        AnalyticsReportRequest request
    );
}