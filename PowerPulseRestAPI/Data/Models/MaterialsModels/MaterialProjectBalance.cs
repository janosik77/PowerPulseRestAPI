using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Models
{


    public class MaterialProjectBalance
        {
            public long MaterialId { get; set; }
            public long ProjectId { get; set; }
            public decimal Quantity { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }

            public Material? Material { get; set; }
            public Project? Project { get; set; }
        }
    
}
