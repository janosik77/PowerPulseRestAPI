using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Models.CustomerModels
{

    public class CustomerContact
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? RoleTitle { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public bool IsPrimary { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Customer? Customer { get; set; }
        public List<ProjectCustomerContact> ProjectLinks { get; set; } = new();

    }
}
