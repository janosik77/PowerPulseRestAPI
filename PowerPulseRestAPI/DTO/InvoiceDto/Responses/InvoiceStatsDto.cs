namespace PowerPulseRestAPI.DTO.InvoiceDto.Responses;

public class InvoiceStatsDto
{
    public InvoiceStatsSummaryDto Summary { get; set; } = new();
    public List<RecentInvoiceStatsItemDto> RecentInvoices { get; set; } = new();
}