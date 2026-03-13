using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.CustomerDto.Response
{
    public sealed class CustomerAddressDto
    {
        public long EntityAddressId { get; set; }
        public AddressType AddressType { get; set; }
        public bool IsPrimary { get; set; }
        public long AddressId { get; set; }
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;
        public string? ApartmentNumber { get; set; }
        public string? FullText { get; set; }
    }
}
