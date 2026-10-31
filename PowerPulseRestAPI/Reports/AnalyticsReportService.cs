using PowerPulseRestAPI.BuissnesLogic.AnalyticsS;
using PowerPulseRestAPI.DTO.AnalyticsDto.Responses;
using PowerPulseRestAPI.Reports.Generators;
using PowerPulseRestAPI.Reports.Models;

namespace PowerPulseRestAPI.Reports;

public class AnalyticsReportService
    : IAnalyticsReportService
{
    private readonly IAnalyticsService
        _analyticsService;

    private readonly IEnumerable<IReportGenerator>
        _generators;

    public AnalyticsReportService(
        IAnalyticsService analyticsService,
        IEnumerable<IReportGenerator> generators)
    {
        _analyticsService =
            analyticsService;

        _generators =
            generators;
    }

    public async Task<GeneratedReport> GenerateAsync(
        AnalyticsReportRequest request,
        string? generatedBy,
        CancellationToken cancellationToken = default)
    {
        Validate(request);

        var format =
            NormalizeFormat(
                request.Format);

        var generator =
            _generators.FirstOrDefault(x =>
                string.Equals(
                    x.Format,
                    format,
                    StringComparison.OrdinalIgnoreCase));

        if (generator == null)
        {
            throw new InvalidOperationException(
                $"Report format '{format}' is not supported.");
        }

        var analytics =
            await _analyticsService
                .GetOverviewAsync(
                    request.Filters,
                    cancellationToken);

        var filterOptions =
            await _analyticsService
                .GetFilterOptionsAsync(
                    cancellationToken);

        var reportData =
            new AnalyticsReportData
            {
                Title =
                    string.IsNullOrWhiteSpace(
                        request.Title)
                        ? "Analytics report"
                        : request.Title.Trim(),

                Subtitle =
                    string.IsNullOrWhiteSpace(
                        request.Subtitle)
                        ? null
                        : request.Subtitle.Trim(),

                GeneratedAt =
                    DateTimeOffset.UtcNow,

                GeneratedBy =
                    string.IsNullOrWhiteSpace(
                        generatedBy)
                        ? "—"
                        : generatedBy,

                ProjectLabel =
                    ResolveLabel(
                        filterOptions.Projects,
                        request.Filters.ProjectId,
                        "All projects"),

                EmployeeLabel =
                    ResolveLabel(
                        filterOptions.Employees,
                        request.Filters.EmployeeId,
                        "All employees"),

                CustomerLabel =
                    ResolveLabel(
                        filterOptions.Customers,
                        request.Filters.CustomerId,
                        "All customers"),

                Analytics =
                    analytics
            };

        var content =
            generator.Generate(
                reportData,
                request);

        var fileName =
            "analytics-report-" +
            DateTime.UtcNow
                .ToString(
                    "yyyyMMdd-HHmmss") +
            "." +
            generator.Extension;

        return new GeneratedReport(
            content,
            generator.ContentType,
            fileName);
    }

    private static string NormalizeFormat(
        string? value)
    {
        var format =
            value?
                .Trim()
                .ToLowerInvariant();

        if (format == "pdf")
        {
            return "pdf";
        }

        if (format == "xlsx" ||
            format == "excel")
        {
            return "xlsx";
        }

        return format ??
               string.Empty;
    }

    private static string ResolveLabel(
        IReadOnlyList<AnalyticsSelectOptionDto> options,
        long? id,
        string fallback)
    {
        if (!id.HasValue)
        {
            return fallback;
        }

        var option =
            options.FirstOrDefault(x =>
                x.Value ==
                id.Value);

        return option?.Label ??
               fallback;
    }

    private static void Validate(
        AnalyticsReportRequest request)
    {
        if (request == null)
        {
            throw new InvalidOperationException(
                "Report request is required.");
        }

        if (request.Filters == null)
        {
            throw new InvalidOperationException(
                "Report filters are required.");
        }

        if (request.Sections == null ||
            !request.Sections.AnySelected())
        {
            throw new InvalidOperationException(
                "Select at least one report section.");
        }

        var format =
            NormalizeFormat(
                request.Format);

        if (format != "pdf" &&
            format != "xlsx")
        {
            throw new InvalidOperationException(
                "Supported report formats are PDF and Excel.");
        }

        if (request.Title != null &&
            request.Title.Length > 120)
        {
            throw new InvalidOperationException(
                "Report title cannot exceed 120 characters.");
        }

        if (request.Subtitle != null &&
            request.Subtitle.Length > 250)
        {
            throw new InvalidOperationException(
                "Report subtitle cannot exceed 250 characters.");
        }
    }
}
