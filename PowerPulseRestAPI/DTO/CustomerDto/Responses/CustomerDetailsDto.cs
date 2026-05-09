using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.AddresesDto.Responses;
using PowerPulseRestAPI.DTO.PersonDto.Responses;
using PowerPulseRestAPI.DTO.ProjectDto.Responses;
using System.ComponentModel.DataAnnotations;


namespace PowerPulseRestAPI.DTO.CustomerDto.Responses
{
    public class CustomerDetailsDto
    {
        public long Id { get; set; }

        [Required]
        public CustomerStatus Status { get; set; }

        [Required, MaxLength(200)]
        public string CompanyName { get; set; } = null!;

        [Phone, MaxLength(30)]
        public string? PhoneNumber { get; set; }

        [Required, MaxLength(50)]
        public string TaxId { get; set; } = null!;

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        public DateTimeOffset UpdatedAt { get; set; }

        [Required]
        public AddressDto Address { get; set; } = null!;

        [Required]
        public CustomerContactPersonDto ContactPerson { get; set; } = null!;

        public List<CustomerProjectListItemDto> Projects { get; set; } = new();
    }
}
