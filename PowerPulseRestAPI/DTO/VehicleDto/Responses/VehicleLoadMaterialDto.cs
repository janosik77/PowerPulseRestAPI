namespace PowerPulseRestAPI.DTO.VehicleDto.Responses
{
    public sealed class VehicleLoadMaterialDto
    {
        public long MaterialId { get; init; }
        public string Name { get; init; } = null!;
        public string? Sku { get; init; }
        public string DefaultUnit { get; init; } = null!;
        public decimal Quantity { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
    }

}
