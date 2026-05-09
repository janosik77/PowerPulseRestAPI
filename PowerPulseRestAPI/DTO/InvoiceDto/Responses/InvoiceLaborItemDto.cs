namespace PowerPulseRestAPI.DTO.InvoiceDto.Responses
{
    public class InvoiceLaborItemDto
    {
        public long Id { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal LineSubtotal { get; set; }
        public decimal LineTax { get; set; }
        public decimal LineTotal { get; set; }
    }
}
