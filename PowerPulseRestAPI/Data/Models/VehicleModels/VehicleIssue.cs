using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.VehicleModels
{


    public class VehicleIssue
    {
        public long Id { get; set; }
        public long VehicleId { get; set; }
        public long ReportedByUserId { get; set; }
        public string Description { get; set; } = null!;
        public GenericStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Vehicle Vehicle { get; set; } = null!;
        public User ReportedByUser { get; set; } = null!;

    }

}
