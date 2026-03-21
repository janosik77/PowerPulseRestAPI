namespace PowerPulseRestAPI.DTO.VehicleDto.Responses
{
    public class VehicleListItemDto
    {
        public long Id { get; set; }
        public string PlateNumber { get; set; } = null!;
        public string Vin { get; set; } = null!;
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string Url { get; set; } = null!;
    }
}
