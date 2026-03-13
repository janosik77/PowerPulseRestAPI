using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.InvoiceModels
{


    public class InvoiceItem
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public InvoiceItemType ItemType { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }

        public decimal LineSubtotal { get; set; }
        public decimal LineTax { get; set; }
        public decimal LineTotal { get; set; }
        public int SortOrder { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Invoice Invoice { get; set; } = null!;

    }

}
