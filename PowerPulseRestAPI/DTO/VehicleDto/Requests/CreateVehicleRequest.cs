namespace PowerPulseRestAPI.DTO.VehicleDto.Requests
{
    public sealed class CreateVehicleRequest
    {
        public string Name { get; init; } = null!;
        public string PlateNumber { get; init; } = null!;
        public string Vin { get; init; } = null!;
        public string Make { get; init; } = null!;
        public string Model { get; init; } = null!;
        public int Year { get; init; }
        public int CurrentMileage { get; set; }
    }
}
