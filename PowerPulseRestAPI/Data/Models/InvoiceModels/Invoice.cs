using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.InvoiceModels
{
    public class Invoice
      {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public InvoiceStatus Status { get; set; }
        public long? ProjectId { get; set; }
        public long CustomerId { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly DueDate { get; set; }
        public string Currency { get; set; } = "PLN";
        public decimal SubtotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Note { get; set; }
        public long CreatedByUserId { get; set; }
        public long? IssuedByUserId { get; set; }
        public DateTimeOffset? IssuedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public string? CustomerNameSnapshot { get; set; }
        public string? CustomerTaxIdSnapshot { get; set; }
        public string? BillingAddressSnapshot { get; set; }

        public Project? Project { get; set; }
        public Customer? Customer { get; set; }
        public User? CreatedByUser { get; set; }
        public User? IssuedByUser { get; set; }
        public List<InvoiceItem> Items { get; set; } = new();
        public List<InvoiceHistory> History { get; set; } = new();

    }
}
