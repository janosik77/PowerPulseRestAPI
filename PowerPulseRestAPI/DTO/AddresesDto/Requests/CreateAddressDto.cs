using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.AddresesDto.Requests
{
    public class CreateAddressDto
    {
        [Required, MaxLength(100)]
        public string Country { get; init; } = null!;

        [Required, MaxLength(20)]
        public string PostalCode { get; init; } = null!;

        [Required, MaxLength(120)]
        public string City { get; init; } = null!;

        [Required, MaxLength(200)]
        public string Street { get; init; } = null!;

        [Required, MaxLength(30)]
        public string BuildingNumber { get; init; } = null!;

        [MaxLength(30)]
        public string? ApartmentNumber { get; init; }

        [MaxLength(500)]
        public string? FullText { get; init; }

        public AddressType AddressType { get; init; }
    }
}