using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.StockRequestModels;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;

namespace PowerPulseRestAPI.Data.Models
{


    public class MaterialMovement
        {
            public long Id { get; set; }
            public long MaterialId { get; set; }
            public MaterialMovementType TransactionType { get; set; }
            public decimal Quantity { get; set; }
            public string Unit { get; set; } = null!;
            public long? FromStorageLocationId { get; set; }
            public long? ToStorageLocationId { get; set; }
            public long? ToProjectId { get; set; }
            public long? ToVehicleId { get; set; }
            public long? ToEmployeeId { get; set; }
            public string? Note { get; set; }
            public DateTimeOffset OccurredAt { get; set; }
            public long CreatedByUserId { get; set; }
            public DateTimeOffset CreatedAt { get; set; }

            public Material? Material { get; set; }
            public StorageLocation? FromStorageLocation { get; set; }
            public StorageLocation? ToStorageLocation { get; set; }
            public Project? ToProject { get; set; }
            public Vehicle? ToVehicle { get; set; }
            public Employee? ToEmployee { get; set; }
            public User? CreatedByUser { get; set; }
        }
    
}
