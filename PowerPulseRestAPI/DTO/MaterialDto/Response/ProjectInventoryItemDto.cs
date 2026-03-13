namespace PowerPulseRestAPI.DTO.MaterialDto.Response
{
    public class ProjectInventoryItemDto
    {
        public long MaterialId { get; set; }
        public string MaterialName { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal SystemQuantity { get; set; }
    }
}
