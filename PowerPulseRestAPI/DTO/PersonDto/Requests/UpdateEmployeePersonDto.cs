using PowerPulseRestAPI.DTO.AddresesDto.Requests;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.PersonDto.Requests
{
    public class UpdateEmployeePersonDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required, Phone, MaxLength(30)]
        public string Phone { get; set; } = null!;
        [Required]
        public DateOnly? DateOfBirth { get; set; }

        [Required, MaxLength(500)]
        public string AvatarUrl { get; set; } = null!;

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        public UpdateAddressDto Address { get; set; } = null!;
    }
}
