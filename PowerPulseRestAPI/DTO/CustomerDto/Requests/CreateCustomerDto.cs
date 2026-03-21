using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.AddressDto.Requests;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.CustomerDto.Requests
{
    public class CreateCustomerDto
    {
        public CustomerStatus Status { get; set; }
        [Required, MaxLength(255)]
        public string CompanyName { get; set; } = null!;
        [Required, MaxLength(50)]
        public string TaxId { get; set; } = null!;
        [Phone, MaxLength(30)]
        public string? PhoneNumber { get; set; }
        [Url, MaxLength(500)]
        public string? AvatarUrl { get; set; }
        [Required]
        public CreatePersonDto Contact { get; set; } = new();
        [Required]
        public CreateAddressDto Address { get; set; } = null!;
    }
}
