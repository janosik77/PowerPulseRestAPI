using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.InvoiceModels
{


    public class InvoiceHistory
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public InvoiceStatus OldStatus { get; set; }
        public InvoiceStatus NewStatus { get; set; }
        public string? Note { get; set; }
        public long ChangedByUserId { get; set; }
        public DateTimeOffset ChangedAt { get; set; }

        public Invoice? Invoice { get; set; }
        public User? ChangedByUser { get; set; }
    }

}
