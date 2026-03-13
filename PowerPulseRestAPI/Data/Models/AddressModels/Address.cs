using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.AddressModels
{
    public class Address
    {
        public long Id { get; set; }
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;
        public string? ApartmentNumber { get; set; }
        public string? FullText { get; set; }
        public long? PersonId {get; set;}
        public long? ProjectId {get; set;}
        public AddressType AddressType { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

    }
}
