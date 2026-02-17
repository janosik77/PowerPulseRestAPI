using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Models.VehicleModels
{


    public class VehicleAssignment
    {
        public long Id { get; set; }
        public long VehicleId { get; set; }
        public long EmployeeId { get; set; }
        public DateTimeOffset AssignedAt { get; set; }
        public DateTimeOffset? ReturnedAt { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Vehicle? Vehicle { get; set; }
        public Employee? Employee { get; set; }
    }

}
