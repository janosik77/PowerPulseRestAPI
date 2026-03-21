namespace PowerPulseRestAPI.DTO.InvoiceDto.Requests
{
    public class UpdateInvoiceDto
    {
        public DateOnly IssueDate { get; set; }
        public DateOnly DueDate { get; set; }
        public string Currency { get; set; } = "PLN";
        public string? Note { get; set; }

        public List<UpdateInvoiceItemDto> Items { get; set; } = new();
    }
}
