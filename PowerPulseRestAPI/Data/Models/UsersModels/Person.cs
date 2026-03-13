using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Models.UsersModels
{
    public class Person
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Phone { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string Email { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public User? User { get; set; }
        public Employee? Employee { get; set; }
        public List<CustomerPerson> CustomerLinks { get; set; } = new();
        public List<PersonIdentifier> Identifiers { get; set; } = new();
    }
}
