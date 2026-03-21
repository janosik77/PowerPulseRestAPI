using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.VehicleDto.Requests
{
    public class CreateVehicleDto
    {
        public string PlateNumber { get; set; } = null!;
        public string Vin { get; set; } = null!;
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public VehicleStatus Status { get; set; }
        public int CurrentMileage { get; set; }
        public DateOnly? LastServiceAt { get; set; }
        public int? LastServiceMileage { get; set; }
    }
}
