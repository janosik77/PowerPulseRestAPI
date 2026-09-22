namespace PowerPulseRestAPI.DTO.EmployeeDto.Responses;

public class EmployeeStatsSummaryDto
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int UnavailableEmployees { get; set; }
    public int NewThisMonth { get; set; } 
}