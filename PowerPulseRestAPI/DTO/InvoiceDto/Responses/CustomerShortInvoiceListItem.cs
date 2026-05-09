using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.InvoiceDto.Responses
{
    public class CustomerShortInvoiceListItem
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public InvoiceStatus Status { get; set; }
        public string ProjectName { get; set; } = null!;
    }
}
