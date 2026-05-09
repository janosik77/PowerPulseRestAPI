using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.EmployeeDto.Responses
{
    public class EmployeeDetailsPublicDto
    {
        public long Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required, Phone, MaxLength(30)]
        public string Phone { get; set; } = null!;

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        public DateOnly HireDate { get; set; }

        public DateOnly? TerminatedAt { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        [Required, MaxLength(100)]
        public string JobTitle { get; set; } = null!;

        [Required]
        public EmployeeStatus Status { get; set; }

    }
}
