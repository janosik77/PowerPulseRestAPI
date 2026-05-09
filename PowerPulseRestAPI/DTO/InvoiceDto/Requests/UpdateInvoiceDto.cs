namespace PowerPulseRestAPI.DTO.InvoiceDto.Requests
{
    public class UpdateInvoiceDto
    {
        public DateOnly DueDate { get; set; }
        public string? Note { get; set; }

    }
}
