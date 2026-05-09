using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.AddresesDto.Requests;
using PowerPulseRestAPI.DTO.PersonDto.Requests;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.CustomerDto.Requests
{
    public class UpdateCustomerDto
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
        public UpdateCustomerPersonDto ContactPerson { get; set; } = null!;
        [Required]
        public UpdateAddressDto Address { get; set; } = null!;
    }
}
