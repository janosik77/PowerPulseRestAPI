using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.VehicleModels
{


    public class VehicleMileageRecord
    {
        public long Id { get; set; }
        public long VehicleId { get; set; }
        public int Mileage { get; set; }
        public DateTimeOffset RecordedAt { get; set; }
        public MileageSourceType SourceType { get; set; }
        public string? Note { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Vehicle Vehicle { get; set; } = null!;
        public User? CreatedByUser { get; set; }
    }

}
