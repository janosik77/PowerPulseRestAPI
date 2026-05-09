using PowerPulseRestAPI.DTO.AddresesDto.Requests;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.PersonDto.Requests
{
    public class UpdateCustomerPersonDto
    {
        [Required, MaxLength(100)]
        public string? FirstName { get; set; }

        [Required, MaxLength(100)]
        public string? LastName { get; set; }

        [Required, Phone, MaxLength(30)]
        public string? Phone { get; set; }

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }
        public DateOnly? DateOfBirth { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string? Email { get; set; }
       
        public UpdateAddressDto? Address { get; set; }
    }
}
