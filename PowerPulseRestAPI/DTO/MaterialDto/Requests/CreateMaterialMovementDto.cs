using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.MaterialDto.Requests
{
    public class CreateMaterialMovementDto
    {
        public long MaterialId { get; set; }
        public MaterialMovementType MovementType { get; set; }
        public long? ProjectId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
        public string? Note { get; set; }
    }
}
