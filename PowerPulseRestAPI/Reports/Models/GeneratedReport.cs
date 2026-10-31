namespace PowerPulseRestAPI.Reports.Models;

public record GeneratedReport(
    byte[] Content,
    string ContentType,
    string FileName
);