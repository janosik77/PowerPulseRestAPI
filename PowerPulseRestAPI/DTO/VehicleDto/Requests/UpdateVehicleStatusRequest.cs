using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.VehicleDto.Requests
{
    public sealed class UpdateVehicleStatusRequest
    {
        public VehicleStatus Status { get; init; }
    }

}
