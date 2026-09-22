namespace PowerPulseRestAPI.DTO.InvoiceDto.Responses;

public class InvoiceStatsSummaryDto
{
    public int TotalInvoices { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PaidInvoices { get; set; }
    public int OverdueInvoices { get; set; }
    public string DominantCurrency { get; set; } = "PLN";
}