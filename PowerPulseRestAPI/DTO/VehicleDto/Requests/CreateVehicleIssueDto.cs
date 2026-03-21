using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.VehicleDto.Requests
{
    public class CreateVehicleIssueDto
    {
        public string Description { get; set; } = null!;
        public GenericStatus Status { get; set; }

    }
}
