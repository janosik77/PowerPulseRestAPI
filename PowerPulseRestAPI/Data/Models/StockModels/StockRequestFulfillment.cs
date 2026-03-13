using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.StockRequestModels
{


    public class StockRequestFulfillment
    {
        public long Id { get; set; }
        public long StockRequestId { get; set; }
        public long IssuedByUserId { get; set; }
        public DateTimeOffset IssuedAt { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public StockRequest? StockRequest { get; set; }
        public User? IssuedByUser { get; set; }
        public List<StockRequestFulfillmentItem> Items { get; set; } = new();

    }

}
