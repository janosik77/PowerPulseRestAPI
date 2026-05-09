using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.AddresesDto.Requests;
using PowerPulseRestAPI.DTO.PersonDto.Requests;
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
        [MaxLength(500)]
        public string? AvatarUrl { get; set; }
        [Required]
        public CreateCustomerPersonDto ContactPerson { get; set; } = null!;
        [Required]
        public CreateAddressDto Address { get; set; } = null!;
    }
}
