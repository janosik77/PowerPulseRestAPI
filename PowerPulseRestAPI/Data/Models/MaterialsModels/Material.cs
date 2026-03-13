
namespace PowerPulseRestAPI.Data.Models.MaterialsModels
{   
        public class Material
        {
            public long Id { get; set; }
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
            public DateTimeOffset CreatedAt { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }

            public MaterialCategory? Category { get; set; }

            public MaterialStock? Stock { get; set; }
            public List<MaterialMovement> Movements { get; set; } = new();
            public List<MaterialProjectBalance> ProjectBalances { get; set; } = new();
            public List<MaterialProjectConsume> ProjectConsumes { get; set; } = new();
    }

}
