using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.VehicleModels
{


    public class ServiceOrderHistory
    {
        public long Id { get; set; }
        public long ServiceOrderId { get; set; }
        public ServiceOrderStatus OldStatus { get; set; }
        public ServiceOrderStatus NewStatus { get; set; }
        public string? Note { get; set; }
        public long ChangedByUserId { get; set; }
        public DateTimeOffset ChangedAt { get; set; }

        public ServiceOrder? ServiceOrder { get; set; }
        public User? ChangedByUser { get; set; }
    }

}
