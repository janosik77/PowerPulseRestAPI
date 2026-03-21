namespace PowerPulseRestAPI.DTO.InvoiceDto.Requests
{
    public class CreateInvoiceDto
    {
        public long ProjectId { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly DueDate { get; set; }
        public string Currency { get; set; } = "PLN";
        public string? Note { get; set; }

        public List<CreateInvoiceItemDto> Items { get; set; } = new();
    }
}
