using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.InvoiceDto.Responses
{
    public class InvoiceDetailsDto
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public InvoiceStatus Status { get; set; }

        public long? ProjectId { get; set; }
        public string? ProjectName { get; set; }

        public long CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;

        public DateOnly IssueDate { get; set; }
        public DateOnly DueDate { get; set; }
        public string Currency { get; set; } = "PLN";

        public decimal SubtotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public string? Note { get; set; }

        public string? CustomerTaxIdSnapshot { get; set; }
        public string? BillingAddressSnapshot { get; set; }

        public string? CreatedByUserName { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public InvoiceLaborItemDto? LaborItem { get; set; }
        public List<InvoiceMaterialItemDto> MaterialItems { get; set; } = new();
    }
}
