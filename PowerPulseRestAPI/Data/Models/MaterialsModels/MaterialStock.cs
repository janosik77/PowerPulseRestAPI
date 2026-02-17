using PowerPulseRestAPI.Data.Models.StockRequestModels;

namespace PowerPulseRestAPI.Data.Models
{


    public class MaterialStock
        {
            public long Id { get; set; }
            public long MaterialId { get; set; }
            public long StorageLocationId { get; set; }
            public decimal Quantity { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }

            public Material? Material { get; set; }
            public StorageLocation? StorageLocation { get; set; }
        }
    
}
