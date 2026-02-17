using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.UsersModels
{
    public class PersonIdentifier
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public IdentifierType Type { get; set; }
        public string ValueEncrypted { get; set; } = null!;
        public string? Last4 { get; set; }
        public string? Country { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Person? Person { get; set; }
    }
}
