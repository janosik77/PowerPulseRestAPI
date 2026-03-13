namespace PowerPulseRestAPI.DTO.VehicleDto.Responses
{
    public sealed class VehicleLoadDto
    {
        public long VehicleId { get; init; }

        public List<VehicleLoadMaterialDto> Materials { get; init; } = new();
        public List<VehicleLoadToolDto> Tools { get; init; } = new();
    }
}
