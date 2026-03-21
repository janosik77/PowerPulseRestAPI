using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.InvoiceDto.Requests
{
    public class UpdateInvoiceStatusDto
    {
        public InvoiceStatus Status { get; set; }
    }
}
