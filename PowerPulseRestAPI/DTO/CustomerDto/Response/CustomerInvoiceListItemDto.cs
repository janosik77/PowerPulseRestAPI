using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.CustomerDto.Response
{
    public class CustomerInvoiceListItemDto
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public InvoiceStatus Status { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly DueDate { get; set; }
        public string Currency { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public long? ProjectId { get; set; }
        public string? ProjectCode { get; set; }
        public string? ProjectName { get; set; }
    }
}
