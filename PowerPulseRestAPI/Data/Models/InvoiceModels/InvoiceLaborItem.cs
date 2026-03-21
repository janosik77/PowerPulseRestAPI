using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.InvoiceModels
{


    public class InvoiceLaborItem
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "h";
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }

        public decimal LineSubtotal { get; set; }
        public decimal LineTax { get; set; }
        public decimal LineTotal { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Invoice Invoice { get; set; } = null!;

    }

}
