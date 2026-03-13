namespace PowerPulseRestAPI.DTO.MaterialDto.Response
{
    public class MaterialMovementDto
    {
        public long Id { get; set; }
        public long MaterialId { get; set; }
        public string MaterialName { get; set; } = null!;
        public string MovementType { get; set; } = null!;
        public long? ProjectId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
        public string? Note { get; set; }
        public DateTimeOffset OccurredAt { get; set; }
        public long CreatedByUserId { get; set; }
    }
}
