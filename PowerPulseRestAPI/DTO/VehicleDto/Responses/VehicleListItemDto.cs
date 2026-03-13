using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.VehicleDto.Responses
{
    public sealed class VehicleListItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string PlateNumber { get; set; } = null!;
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public VehicleStatus Status { get; set; }
        public int? CurrentMileage { get; set; }
    }
}
