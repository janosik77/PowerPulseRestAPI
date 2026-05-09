using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.InvoiceDto.Requests
{
    public class CreateInvoiceItemDto
    {
        public InvoiceFormItemType ItemType { get; set; }

        public long? MaterialId { get; set; }

        public decimal? UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        
    }
}
