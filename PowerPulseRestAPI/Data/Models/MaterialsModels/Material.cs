
using PowerPulseRestAPI.Data.Models.InvoiceModels;

namespace PowerPulseRestAPI.Data.Models.MaterialsModels
{   
        public class Material
        {
            public long Id { get; set; }
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
            public bool IsDeleted { get; set; }

            public MaterialCategory? Category { get; set; }
            public List<InvoiceMaterialItem> InvoiceMaterialItems { get; set; } = new();
            public List<MaterialMovement> Movements { get; set; } = new();
    }

}
