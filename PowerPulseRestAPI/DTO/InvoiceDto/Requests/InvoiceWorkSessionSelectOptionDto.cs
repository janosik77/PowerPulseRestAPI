namespace PowerPulseRestAPI.DTO.InvoiceDto.Requests;

public class InvoiceWorkSessionSelectOptionDto
{
    public long Id { get; set; }
    
    public long EmployeeId { get; set; }
    
    public string EmployeeName { get; set; } = string.Empty;
    
    public DateTimeOffset StartedAt { get; set; }
    
    public DateTimeOffset EndedAt { get; set; }
    
    public decimal Hours { get; set; }
    
}