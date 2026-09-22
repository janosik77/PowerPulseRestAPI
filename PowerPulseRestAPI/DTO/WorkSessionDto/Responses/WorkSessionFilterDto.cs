namespace PowerPulseRestAPI.DTO.WorkSessionDto.Responses;

public class WorkSessionFilterDto
{
    public string? Search { get; set; }
    public long? ProjectId { get; set; }
    public int? Status { get; set; }

    // "", "invoiced", "notInvoiced"
    public string? InvoiceStatus { get; set; }

    // "all", "7d", "30d", "90d"
    public string? DateRange { get; set; } = "all";
}