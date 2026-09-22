namespace PowerPulseRestAPI.DTO.VehicleDto.Responses;

public class VehicleStatsDto
{
    public VehicleStatsSummaryDto Summary { get; set; } = new();
    public List<VehicleRecentAlertDto> RecentAlerts { get; set; } = new();
}