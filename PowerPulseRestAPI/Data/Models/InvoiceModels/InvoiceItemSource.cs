using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.InvoiceModels
{

    public class InvoiceItemSource
    {
        public long Id { get; set; }
        public long InvoiceItemId { get; set; }
        public InvoiceSourceType SourceType { get; set; }
        public long SourceId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }

        public InvoiceItem? InvoiceItem { get; set; }
    }

}
