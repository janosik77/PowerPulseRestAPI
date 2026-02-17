using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.StockRequestModels;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;

namespace PowerPulseRestAPI.Data.Models.ToolsModels
{

    public class ToolAssignment
    {
        public long Id { get; set; }
        public long ToolAssetId { get; set; }
        public long? ToStorageLocationId { get; set; }
        public long? ToVehicleId { get; set; }
        public long? ToProjectId { get; set; }
        public long? ToEmployeeId { get; set; }
        public DateTimeOffset AssignedAt { get; set; }
        public DateTimeOffset? ReturnedAt { get; set; }
        public string? Notes { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public ToolAsset? ToolAsset { get; set; }
        public StorageLocation? ToStorageLocation { get; set; }
        public Vehicle? ToVehicle { get; set; }
        public Project? ToProject { get; set; }
        public Employee? ToEmployee { get; set; }
        public User? CreatedByUser { get; set; }
    }

}
