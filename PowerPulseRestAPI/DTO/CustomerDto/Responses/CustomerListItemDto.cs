using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.CustomerDto.Responses
{
    public class CustomerListItemDto
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

        [Required, MaxLength(200)]
        public string ContactPersonFullName { get; set; } = null!;


        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
