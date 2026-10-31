using PowerPulseRestAPI.Reports.Documents;
using PowerPulseRestAPI.Reports.Models;
using QuestPDF.Fluent;

namespace PowerPulseRestAPI.Reports.Generators;

public class QuestPdfReportGenerator : IReportGenerator
{
    public string Format => "pdf";

    public string ContentType => "application/pdf";

    public string Extension => "pdf";

    public byte[] Generate(
        AnalyticsReportData data,
        AnalyticsReportRequest request)
    {
        var document = new AnalyticsReportDocument(data, request);

        return document.GeneratePdf();
    }
}