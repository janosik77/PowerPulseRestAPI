using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data.Models.PersonModels;

namespace PowerPulseRestAPI.DTO.AddresesDto.Responses
{
    public class AddressDto
    {
        public long Id { get; set; }
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;
        public string? ApartmentNumber { get; set; }
        public AddressType AddressType { get; set; }
        public string? FullText { get; set; }

    }
}
