using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.VehicleDto.Requests
{
    public sealed class UpdateVehicleRequest
    {
        public string Name { get; init; } = null!;
        public string PlateNumber { get; init; } = null!;
        public string? Vin { get; init; }
        public string? Make { get; init; }
        public string? Model { get; init; }
        public int? Year { get; init; }
        public VehicleStatus Status { get; init; }
        public int? CurrentMileage { get; init; }
        public DateOnly? LastServiceAt { get; init; }
        public int? LastServiceMileage { get; init; }
    }
}
