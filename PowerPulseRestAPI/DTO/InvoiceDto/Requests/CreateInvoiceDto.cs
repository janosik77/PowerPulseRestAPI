using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.InvoiceDto.Requests
{
    public class CreateInvoiceDto
    {
        [Range(1, long.MaxValue)]
        public long ProjectId { get; set; }

        [Required]
        public DateOnly IssueDate { get; set; }

        [Required]
        public DateOnly DueDate { get; set; }

        [Required, MaxLength(3)]
        public string Currency { get; set; } = "PLN";

        [MaxLength(2000)]
        public string? Note { get; set; }

        [Required]
        public DateOnly BillingPeriodStart { get; set; }

        [Required]
        public DateOnly BillingPeriodEnd { get; set; }

        [Required, MinLength(1)]
        public List<CreateInvoiceItemDto> Items { get; set; } = new();
    }
}
