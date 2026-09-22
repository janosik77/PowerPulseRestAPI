using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.InvoiceDto.Responses;

public class RecentInvoiceStatsItemDto
{
    public long Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string? ProjectName { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "PLN";
    public InvoiceStatus Status { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}