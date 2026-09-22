namespace PowerPulseRestAPI.DTO.WorkSessionDto.Responses;

public class WorkSessionStatsSummaryDto
{
    public int TotalWorkSessions { get; set; }

    public long TotalMillisecondsWorked { get; set; }
    public double TotalHoursWorked { get; set; }
    public string TotalHoursWorkedLabel { get; set; } = "0h 0m";

    public int ClosedSessions { get; set; }
    public int NotInvoicedSessions { get; set; }
}