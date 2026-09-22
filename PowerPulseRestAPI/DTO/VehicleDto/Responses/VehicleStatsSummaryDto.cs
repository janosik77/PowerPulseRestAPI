namespace PowerPulseRestAPI.DTO.VehicleDto.Responses;

public class VehicleStatsSummaryDto
{
    public int TotalVehicles { get; set; }
    public int ActiveVehicles { get; set; }
    public int VehiclesWithIssues { get; set; }
    public int TotalReports { get; set; }
}