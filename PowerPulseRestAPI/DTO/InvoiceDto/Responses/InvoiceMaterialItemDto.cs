namespace PowerPulseRestAPI.DTO.InvoiceDto.Responses
{
    public class InvoiceMaterialItemDto
    {
        public long Id { get; set; }
        public long MaterialId { get; set; }
        public string MaterialName { get; set; } = null!;
        public string MaterialSku { get; set; } = null!;

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }

        public decimal LineSubtotal { get; set; }
        public decimal LineTax { get; set; }
        public decimal LineTotal { get; set; }
    }
}
