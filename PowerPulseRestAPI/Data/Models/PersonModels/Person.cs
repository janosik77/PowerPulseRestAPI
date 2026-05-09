using PowerPulseRestAPI.Data.Models.AddressModels;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.PersonModels
{
    public class Person
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string Email { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public long? AddressId { get; set; }
        public bool IsDeleted { get; set; }

        public Address? Address { get; set; }
        public User? User { get; set; }
        public Employee? Employee { get; set; }
        public List<PersonIdentifier> Identifiers { get; set; } = new();
    }
}
