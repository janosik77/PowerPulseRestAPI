using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.VehicleDto.Responses;

public class VehicleFilterDto
{
    public string? Search { get; set; }

    public VehicleStatus? Status { get; set; }

    public string? Make { get; set; }
}