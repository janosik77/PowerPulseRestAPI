using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Models.MaterialsModels
{


    public class MaterialProjectBalance
        {
            public long MaterialId { get; set; }
            public long ProjectId { get; set; }
            public decimal Quantity { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }

            public byte[] RowVersion { get; set; } = null!;

            public Material Material { get; set; } = null!;
            public Project Project { get; set; } = null!;
        }
    
}
