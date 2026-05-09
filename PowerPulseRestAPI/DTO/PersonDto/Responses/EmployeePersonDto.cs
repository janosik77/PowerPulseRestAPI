using PowerPulseRestAPI.DTO.AddresesDto.Responses;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.PersonDto.Responses
{
    public class EmployeePersonDto
    {
        public long Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required, Phone, MaxLength(30)]
        public string Phone { get; set; } = null!;
        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required, Url, MaxLength(500)]
        public string AvatarUrl { get; set; } = null!;

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;
        [Required]
        public AddressDto Address { get; set; } = null!;
    }
}
