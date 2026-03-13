namespace PowerPulseRestAPI.DTO.MaterialDto.Response
{
    public class MaterialBalanceDto
    {
        public long MaterialId { get; set; }
        public string MaterialName { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public string? Barcode { get; set; }
        public string Unit { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public bool IsActive { get; set; }
        public long? StorageLocationId { get; set; }
    }
}
