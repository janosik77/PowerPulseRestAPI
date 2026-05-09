using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.PersonModels
{
    public class PersonIdentifier
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public string EncryptedSSN { get; set; } = null!;
        public string? Last4 { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Person? Person { get; set; }
    }
}
