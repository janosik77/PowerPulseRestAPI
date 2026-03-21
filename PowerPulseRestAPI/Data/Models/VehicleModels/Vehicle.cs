using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.VehicleModels
{


    public class Vehicle
    {
        public long Id { get; set; }
        public string PlateNumber { get; set; } = null!;
        public string Vin { get; set; } = null!;
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public VehicleStatus Status { get; set; }
        public int CurrentMileage { get; set; }
        public DateOnly? LastServiceAt { get; set; }
        public int? LastServiceMileage { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public List<VehicleAssignment> Assignments { get; set; } = new();
        public List<VehicleIssue> Issues { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();


    }

}
