namespace PowerPulseRestAPI.DTO.MaterialDto.Request
{
    public class UpdateMaterialRequest
    {
        public string Sku { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long CategoryId { get; set; }
        public string? Manufacturer { get; set; }
        public string? Barcode { get; set; }
        public string DefaultUnit { get; set; } = null!;
        public string Url { get; set; } = null!;
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
