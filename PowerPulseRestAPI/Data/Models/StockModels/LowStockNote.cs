using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Models.StockModels
{


    public class LowStockNote
    {
        public long Id { get; set; }
        public PurchasePriority Priority { get; set; }
        public long CreatedByEmployeeId { get; set; }
        public string Note { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Employee CreatedByEmployee { get; set; } = null!;


    }

}
