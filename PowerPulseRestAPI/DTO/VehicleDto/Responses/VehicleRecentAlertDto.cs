namespace PowerPulseRestAPI.DTO.VehicleDto.Responses;

public class VehicleRecentAlertDto
{
    public long VehicleId { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int ReportsCount { get; set; }
}