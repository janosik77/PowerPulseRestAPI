namespace PowerPulseRestAPI.DTO.MaterialDto.Response
{
    public class MaterialDto
    {
        public long Id { get; set; }
        public string Sku { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Manufacturer { get; set; }
        public string? Barcode { get; set; }
        public string DefaultUnit { get; set; } = null!;
        public bool IsActive { get; set; }
        public string Url { get; set; } = null!;
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
