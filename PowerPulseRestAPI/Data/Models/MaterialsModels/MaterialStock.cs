
namespace PowerPulseRestAPI.Data.Models.MaterialsModels
{


    public class MaterialStock
        {
            public long MaterialId { get; set; }
            public decimal Quantity { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }
            public byte[] RowVersion { get; set; } = null!;

            public Material Material { get; set; } = null!;
        }
    
}
