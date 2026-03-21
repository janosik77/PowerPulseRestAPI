namespace PowerPulseRestAPI.DTO.MaterialDto.Responses
{
    public class ProjectMaterialConsumeDto
    {
        public long ProjectId { get; set; }
        public long MaterialId { get; set; }
        public string MaterialName { get; set; } = null!;
        public string? Manufacturer { get; set; }
        public string Url { get; set; } = null!;
        public decimal ConsumedQuantity { get; set; }
        public string Unit { get; set; } = null!;
    }
}
