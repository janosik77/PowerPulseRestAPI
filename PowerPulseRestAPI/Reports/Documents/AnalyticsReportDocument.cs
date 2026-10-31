using System.Globalization;
using System.Net;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PowerPulseRestAPI.Reports.Models;

namespace PowerPulseRestAPI.Reports.Documents;

public class AnalyticsReportDocument : IDocument
{
    private readonly AnalyticsReportData _report;
    private readonly AnalyticsReportRequest _request;

    private const string Dark = "#0B192C";
    private const string Orange = "#D96B2B";
    private const string Blue = "#4F8FBD";
    private const string Purple = "#8E63B0";
    private const string Green = "#5B9B78";

    public AnalyticsReportDocument(
        AnalyticsReportData report,
        AnalyticsReportRequest request)
    {
        _report = report;
        _request = request;
    }

    public DocumentMetadata GetMetadata()
    {
        return DocumentMetadata.Default;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);

            page.Margin(28);

            page.PageColor(Colors.White);

            page.DefaultTextStyle(style =>
                style
                    .FontSize(9)
                    .FontColor("#243447"));

            page.Header()
                .Element(ComposeHeader);

            page.Content()
                .PaddingVertical(14)
                .Column(column =>
                {
                    column.Spacing(16);

                    column.Item()
                        .Element(ComposeScope);

                    if (_request.Sections.Summary)
                    {
                        column.Item()
                            .Element(ComposeSummary);
                    }

                    if (_request.Sections.WorkHours)
                    {
                        column.Item()
                            .Element(ComposeWorkHours);
                    }

                    if (_request.Sections.Projects)
                    {
                        column.Item()
                            .Element(ComposeProjects);
                    }

                    if (_request.Sections.Materials)
                    {
                        column.Item()
                            .Element(ComposeMaterials);
                    }

                    if (_request.Sections.Invoices)
                    {
                        column.Item()
                            .Element(ComposeInvoices);
                    }

                    if (_request.Sections.Employees)
                    {
                        column.Item()
                            .Element(ComposeEmployees);
                    }
                });

            page.Footer()
                .AlignCenter()
                .DefaultTextStyle(style =>
                    style
                        .FontSize(8)
                        .FontColor("#8492A0"))
                .Text(text =>
                {
                    text.Span("PowerPulse · ");

                    text.CurrentPageNumber();

                    text.Span(" / ");

                    text.TotalPages();
                });
        });
    }

    private void ComposeHeader(IContainer container)
    {
        container
            .Background(Dark)
            .Padding(16)
            .Row(row =>
            {
                row.RelativeItem()
                    .Column(column =>
                    {
                        column.Item()
                            .Text(_report.Title)
                            .FontSize(20)
                            .SemiBold()
                            .FontColor(Colors.White);

                        if (!string.IsNullOrWhiteSpace(
                                _report.Subtitle))
                        {
                            column.Item()
                                .PaddingTop(3)
                                .Text(_report.Subtitle)
                                .FontSize(9)
                                .FontColor("#AAB7C4");
                        }
                    });

                row.AutoItem()
                    .AlignRight()
                    .Text(
                        _report.GeneratedAt
                            .ToLocalTime()
                            .ToString(
                                "dd MMM yyyy, HH:mm"))
                    .FontSize(8)
                    .FontColor("#AAB7C4");
            });
    }

    private void ComposeScope(IContainer container)
    {
        var period =
            _report.Analytics.Period;

        container
            .Border(1)
            .BorderColor("#DCE5EC")
            .Background("#F7F9FB")
            .Padding(10)
            .Column(column =>
            {
                column.Spacing(4);

                column.Item()
                    .Text("REPORT SCOPE")
                    .FontSize(8)
                    .SemiBold()
                    .FontColor(Orange);

                column.Item()
                    .Text(
                        $"{period.From:dd MMM yyyy} - " +
                        $"{period.To:dd MMM yyyy}");

                column.Item()
                    .Text(
                        $"Project: {_report.ProjectLabel} · " +
                        $"Employee: {_report.EmployeeLabel}");

                column.Item()
                    .Text(
                        $"Customer: {_report.CustomerLabel} · " +
                        $"Currency: {period.Currency}");

                column.Item()
                    .Text(
                        $"Generated by: {_report.GeneratedBy}")
                    .FontSize(8)
                    .FontColor("#748393");
            });
    }

    private void ComposeSummary(IContainer container)
    {
        var summary =
            _report.Analytics.Summary;

        container.Column(column =>
        {
            SectionTitle(
                column,
                "Summary");

            column.Item()
                .Row(row =>
                {
                    row.Spacing(6);

                    row.RelativeItem()
                        .Element(x =>
                            Kpi(
                                x,
                                "Active projects",
                                summary.ActiveProjects.ToString()));

                    row.RelativeItem()
                        .Element(x =>
                            Kpi(
                                x,
                                "Work hours",
                                $"{summary.WorkHours:0.##} h"));

                    row.RelativeItem()
                        .Element(x =>
                            Kpi(
                                x,
                                "Invoice value",
                                $"{summary.InvoiceValue:N2} {summary.Currency}"));

                    row.RelativeItem()
                        .Element(x =>
                            Kpi(
                                x,
                                "Material cost",
                                $"{summary.MaterialCost:N2} {summary.Currency}"));
                });
        });
    }

    private void ComposeWorkHours(IContainer container)
    {
        var data =
            _report.Analytics.WorkHoursTimeline;

        container.Column(column =>
        {
            SectionTitle(
                column,
                "Work hours over time");

            if (_request.IncludeCharts)
            {
                var svg =
                    BuildLineChart(
                        data.Select(x =>
                            (
                                DateLabel(x.Date),
                                x.Value
                            )),
                        Orange);

                column.Item()
                    .Svg(svg)
                    .FitWidth();
            }

            Table(
                column.Item(),
                new[]
                {
                    "Date",
                    "Hours"
                },
                data.Select(x =>
                    new[]
                    {
                        x.Date.ToString(
                            "dd MMM yyyy"),
                        $"{x.Value:0.##}"
                    }));
        });
    }

    private void ComposeProjects(IContainer container)
    {
        container.Column(column =>
        {
            SectionTitle(
                column,
                "Projects");

            if (_request.IncludeCharts)
            {
                column.Item()
                    .Text("Hours by project")
                    .SemiBold()
                    .FontSize(10);

                var hoursSvg =
                    BuildHorizontalBarChart(
                        _report.Analytics
                            .ProjectHours
                            .Select(x =>
                                (
                                    x.ProjectName,
                                    x.Hours
                                )),
                        Orange);

                column.Item()
                    .Svg(hoursSvg)
                    .FitWidth();

                column.Item()
                    .PaddingTop(8)
                    .Text("Projects by status")
                    .SemiBold()
                    .FontSize(10);

                var statusesSvg =
                    BuildHorizontalBarChart(
                        _report.Analytics
                            .ProjectStatuses
                            .Select(x =>
                                (
                                    x.Label,
                                    x.Value
                                )),
                        "#B96A3A");

                column.Item()
                    .Svg(statusesSvg)
                    .FitWidth();
            }

            Table(
                column.Item(),
                new[]
                {
                    "Project",
                    "Hours"
                },
                _report.Analytics
                    .ProjectHours
                    .Select(x =>
                        new[]
                        {
                            x.ProjectName,
                            $"{x.Hours:0.##}"
                        }));
        });
    }

    private void ComposeMaterials(IContainer container)
    {
        var data =
            _report.Analytics.MaterialConsumption;

        container.Column(column =>
        {
            SectionTitle(
                column,
                "Material consumption");

            if (_request.IncludeCharts)
            {
                var svg =
                    BuildHorizontalBarChart(
                        data.Select(x =>
                            (
                                x.MaterialName,
                                x.TotalValue
                            )),
                        Blue);

                column.Item()
                    .Svg(svg)
                    .FitWidth();
            }

            Table(
                column.Item(),
                new[]
                {
                    "Material",
                    "Quantity",
                    "Unit",
                    "Value"
                },
                data.Select(x =>
                    new[]
                    {
                        x.MaterialName,
                        $"{x.Quantity:0.###}",
                        x.Unit,
                        $"{x.TotalValue:N2} {x.Currency}"
                    }));
        });
    }

    private void ComposeInvoices(IContainer container)
    {
        var data =
            _report.Analytics.InvoiceTimeline;

        container.Column(column =>
        {
            SectionTitle(
                column,
                "Invoice value over time");

            if (_request.IncludeCharts)
            {
                var svg =
                    BuildLineChart(
                        data.Select(x =>
                            (
                                DateLabel(x.Date),
                                x.Value
                            )),
                        Green);

                column.Item()
                    .Svg(svg)
                    .FitWidth();
            }

            Table(
                column.Item(),
                new[]
                {
                    "Date",
                    "Value"
                },
                data.Select(x =>
                    new[]
                    {
                        x.Date.ToString(
                            "dd MMM yyyy"),
                        $"{x.Value:N2} {_report.Analytics.Period.Currency}"
                    }));
        });
    }

    private void ComposeEmployees(IContainer container)
    {
        var data =
            _report.Analytics.EmployeeHours;

        container.Column(column =>
        {
            SectionTitle(
                column,
                "Employee workload");

            if (_request.IncludeCharts)
            {
                var svg =
                    BuildHorizontalBarChart(
                        data.Select(x =>
                            (
                                x.EmployeeName,
                                x.Hours
                            )),
                        Purple);

                column.Item()
                    .Svg(svg)
                    .FitWidth();
            }

            Table(
                column.Item(),
                new[]
                {
                    "Employee",
                    "Hours"
                },
                data.Select(x =>
                    new[]
                    {
                        x.EmployeeName,
                        $"{x.Hours:0.##}"
                    }));
        });
    }

    private static void SectionTitle(
        ColumnDescriptor column,
        string title)
    {
        column.Item()
            .PaddingBottom(6)
            .Text(title)
            .FontSize(14)
            .SemiBold()
            .FontColor(Dark);
    }

    private static void Kpi(
        IContainer container,
        string label,
        string value)
    {
        container
            .Border(1)
            .BorderColor("#DCE5EC")
            .Background("#F7F9FB")
            .Padding(9)
            .Column(column =>
            {
                column.Item()
                    .Text(
                        label.ToUpperInvariant())
                    .FontSize(7)
                    .FontColor("#748393");

                column.Item()
                    .PaddingTop(3)
                    .Text(value)
                    .FontSize(12)
                    .SemiBold()
                    .FontColor(Dark);
            });
    }

    private static void Table(
        IContainer container,
        string[] headers,
        IEnumerable<string[]> rows)
    {
        var values =
            rows.ToList();

        container
            .PaddingTop(8)
            .Table(table =>
            {
                table.ColumnsDefinition(
                    columns =>
                    {
                        foreach (var _ in headers)
                        {
                            columns.RelativeColumn();
                        }
                    });

                table.Header(header =>
                {
                    foreach (var title in headers)
                    {
                        header.Cell()
                            .Background(Dark)
                            .Padding(5)
                            .Text(title)
                            .SemiBold()
                            .FontSize(8)
                            .FontColor(
                                Colors.White);
                    }
                });

                foreach (var row in values)
                {
                    foreach (var value in row)
                    {
                        table.Cell()
                            .BorderBottom(1)
                            .BorderColor("#E7EDF2")
                            .Padding(5)
                            .Text(
                                value ?? "—")
                            .FontSize(8);
                    }
                }
            });
    }

    private string DateLabel(DateOnly date)
    {
        return _report.Analytics
                   .Period
                   .Granularity == "month"
            ? date.ToString("MMM yy")
            : date.ToString("dd MMM");
    }

    private static string BuildHorizontalBarChart(
        IEnumerable<(string Label, decimal Value)> source,
        string color)
    {
        var data =
            source
                .Where(x => x.Value > 0)
                .OrderByDescending(
                    x => x.Value)
                .Take(10)
                .ToList();

        const int width = 720;
        const int labelWidth = 190;
        const int right = 65;

        var height =
            Math.Max(
                110,
                data.Count * 34 + 25);

        if (data.Count == 0)
        {
            return EmptySvg(
                width,
                height);
        }

        var max =
            data.Max(x => x.Value);

        var chartWidth =
            width -
            labelWidth -
            right;

        var svg =
            new StringBuilder();

        svg.Append(
            $"<svg xmlns=\"http://www.w3.org/2000/svg\" " +
            $"viewBox=\"0 0 {width} {height}\">");

        for (var i = 0;
             i < data.Count;
             i++)
        {
            var item =
                data[i];

            var y =
                15 +
                i * 34;

            var barWidth =
                max <= 0
                    ? 0
                    : (double)(
                        item.Value / max) *
                      chartWidth;

            var label =
                item.Label.Length > 27
                    ? item.Label.Substring(
                          0,
                          26) + "…"
                    : item.Label;

            svg.AppendFormat(
                CultureInfo.InvariantCulture,
                "<text x=\"0\" y=\"{0}\" " +
                "font-size=\"10\" " +
                "fill=\"#526273\">{1}</text>",
                y + 13,
                WebUtility.HtmlEncode(
                    label));

            svg.AppendFormat(
                CultureInfo.InvariantCulture,
                "<rect x=\"{0}\" y=\"{1}\" " +
                "width=\"{2}\" height=\"18\" " +
                "rx=\"4\" fill=\"{3}\" />",
                labelWidth,
                y,
                barWidth,
                color);

            svg.AppendFormat(
                CultureInfo.InvariantCulture,
                "<text x=\"{0}\" y=\"{1}\" " +
                "font-size=\"9\" " +
                "fill=\"#526273\">{2:0.##}</text>",
                labelWidth +
                barWidth +
                7,
                y + 13,
                item.Value);
        }

        svg.Append("</svg>");

        return svg.ToString();
    }

    private static string BuildLineChart(
        IEnumerable<(string Label, decimal Value)> source,
        string color)
    {
        var data =
            source.ToList();

        const int width = 720;
        const int height = 220;

        const int left = 45;
        const int right = 20;
        const int top = 20;
        const int bottom = 35;

        if (data.Count == 0)
        {
            return EmptySvg(
                width,
                height);
        }

        var max =
            Math.Max(
                1m,
                data.Max(x => x.Value));

        var chartWidth =
            width - left - right;

        var chartHeight =
            height - top - bottom;

        var svg =
            new StringBuilder();

        svg.Append(
            $"<svg xmlns=\"http://www.w3.org/2000/svg\" " +
            $"viewBox=\"0 0 {width} {height}\">");

        for (var i = 0; i <= 4; i++)
        {
            var y =
                top +
                chartHeight -
                chartHeight * i / 4d;

            var value =
                max *
                i /
                4m;

            svg.AppendFormat(
                CultureInfo.InvariantCulture,
                "<line x1=\"{0}\" y1=\"{1}\" " +
                "x2=\"{2}\" y2=\"{1}\" " +
                "stroke=\"#E4EAF0\" " +
                "stroke-width=\"1\" />",
                left,
                y,
                width - right);

            svg.AppendFormat(
                CultureInfo.InvariantCulture,
                "<text x=\"0\" y=\"{0}\" " +
                "font-size=\"9\" " +
                "fill=\"#748393\">{1:0.##}</text>",
                y + 3,
                value);
        }

        var points =
            new List<string>();

        for (var i = 0;
             i < data.Count;
             i++)
        {
            var x =
                data.Count == 1
                    ? left +
                      chartWidth / 2d
                    : left +
                      chartWidth *
                      i /
                      (data.Count - 1d);

            var y =
                top +
                chartHeight -
                (double)(
                    data[i].Value / max) *
                chartHeight;

            points.Add(
                FormattableString.Invariant(
                    $"{x:0.##},{y:0.##}"));

            svg.AppendFormat(
                CultureInfo.InvariantCulture,
                "<circle cx=\"{0}\" cy=\"{1}\" " +
                "r=\"3\" fill=\"{2}\" />",
                x,
                y,
                color);

            var shouldRenderLabel =
                data.Count <= 12 ||
                i == 0 ||
                i == data.Count - 1 ||
                i % Math.Max(
                    1,
                    data.Count / 8) == 0;

            if (shouldRenderLabel)
            {
                svg.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "<text x=\"{0}\" y=\"{1}\" " +
                    "text-anchor=\"middle\" " +
                    "font-size=\"8\" " +
                    "fill=\"#748393\">{2}</text>",
                    x,
                    height - 8,
                    WebUtility.HtmlEncode(
                        data[i].Label));
            }
        }

        svg.Append(
            $"<polyline " +
            $"points=\"{string.Join(" ", points)}\" " +
            $"fill=\"none\" " +
            $"stroke=\"{color}\" " +
            $"stroke-width=\"2.5\" />");

        svg.Append("</svg>");

        return svg.ToString();
    }

    private static string EmptySvg(
        int width,
        int height)
    {
        return
            $"<svg xmlns=\"http://www.w3.org/2000/svg\" " +
            $"viewBox=\"0 0 {width} {height}\">" +
            "<text " +
            "x=\"50%\" " +
            "y=\"50%\" " +
            "text-anchor=\"middle\" " +
            "font-size=\"12\" " +
            "fill=\"#8492A0\">" +
            "No data" +
            "</text>" +
            "</svg>";
    }
}
