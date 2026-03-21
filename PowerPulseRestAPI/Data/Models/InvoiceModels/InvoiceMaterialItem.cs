using PowerPulseRestAPI.Data.Models.MaterialsModels;

namespace PowerPulseRestAPI.Data.Models.InvoiceModels
{
    public class InvoiceMaterialItem
    {
        public long Id { get; set; }

        public long InvoiceId { get; set; }
        public long MaterialId { get; set; }

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }

        public decimal LineSubtotal { get; set; }
        public decimal LineTax { get; set; }
        public decimal LineTotal { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Invoice Invoice { get; set; } = null!;
        public Material Material { get; set; } = null!;
    }
}
