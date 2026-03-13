using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.StockRequestModels
{


    public class StockRequestHistory
    {
        public long Id { get; set; }
        public long StockRequestId { get; set; }
        public StockRequestStatus OldStatus { get; set; }
        public StockRequestStatus NewStatus { get; set; }
        public string? Note { get; set; }
        public long ChangedByUserId { get; set; }
        public DateTimeOffset ChangedAt { get; set; }

        public StockRequest StockRequest { get; set; } = null!;
        public User? ChangedByUser { get; set; }
    }

}
