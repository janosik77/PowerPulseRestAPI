namespace PowerPulseRestAPI.DTO.InvoiceDto.Responses
{
    public class InvoiceMaterialSelectOptionDto
    {
        public long MaterialId { get; set; }
        public string Label { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
    }
}
