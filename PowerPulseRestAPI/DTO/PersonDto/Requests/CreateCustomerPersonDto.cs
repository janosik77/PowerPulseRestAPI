using System.ComponentModel.DataAnnotations;
using PowerPulseRestAPI.DTO.AddresesDto.Requests;

namespace PowerPulseRestAPI.DTO.PersonDto.Requests
{
    public class CreateCustomerPersonDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required, Phone, MaxLength(30)]
        public string Phone { get; set; } = null!;

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;

        public CreateAddressDto? Address { get; set; }
    }
}