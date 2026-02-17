using PowerPulseRestAPI.Data.Models.VehicleModels;

namespace PowerPulseRestAPI.Data.Models
{


    public class MaterialVehicleBalance
        {
            public long MaterialId { get; set; }
            public long VehicleId { get; set; }
            public decimal Quantity { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }

            public Material? Material { get; set; }
            public Vehicle? Vehicle { get; set; }
        }
    
}
