namespace PowerPulseRestAPI.Data.Models.EmployeeModels
{


    public class EmployeeCompensation
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string HourlyWageEncrypted { get; set; } = null!;
        public string Currency { get; set; } = "PLN";
        public DateOnly ValidFrom { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Employee Employee { get; set; } = null!;
    }

}
