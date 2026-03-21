namespace PowerPulseRestAPI.DTO.MaterialDto.Requests
{
    public class CreateMaterialDto
    {
        public string Sku { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long CategoryId { get; set; }
        public string? Manufacturer { get; set; }
        public string? Barcode { get; set; }
        public string DefaultUnit { get; set; } = "pcs";
        public bool IsActive { get; set; } = true;
        public string Url { get; set; } = null!;
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
    }
}
