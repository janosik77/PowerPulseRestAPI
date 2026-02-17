namespace PowerPulseRestAPI.Data.Models.EmployeeModels
{

    public class EmployeeBankAccount
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string AccountEncrypted { get; set; } = null!;
        public string? AccountLast4 { get; set; }
        public string? Country { get; set; }
        public bool IsPrimary { get; set; }
        public DateOnly? ValidFrom { get; set; }
        public DateOnly? ValidTo { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Employee? Employee { get; set; }
    }

}
