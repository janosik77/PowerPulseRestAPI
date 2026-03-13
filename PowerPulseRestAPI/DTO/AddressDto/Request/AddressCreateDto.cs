using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.AddressDto.Request
{
    public class AddressCreateDto
    {
        [Required, MaxLength(100)] public string Country { get; init; } = null!;
        [Required, MaxLength(20)] public string PostalCode { get; init; } = null!;
        [Required, MaxLength(120)] public string City { get; init; } = null!;
        [Required, MaxLength(200)] public string Street { get; init; } = null!;
        [Required, MaxLength(30)] public string BuildingNumber { get; init; } = null!;
        [MaxLength(30)] public string? ApartmentNumber { get; init; }

        public decimal? Latitude { get; init; }
        public decimal? Longitude { get; init; }
    }
}
