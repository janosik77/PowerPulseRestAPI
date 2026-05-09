using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.EmployeeDto.Responses
{
    public class EmployeeListItemDto
    {
        public long Id { get; set; }

        [Required, MaxLength(200)]
        public string FullName { get; set; } = null!;

        [Required, Phone, MaxLength(30)]
        public string Phone { get; set; } = null!;

        [Required, MaxLength(100)]
        public string JobTitle { get; set; } = null!;

        [Required]
        public EmployeeStatus Status { get; set; }

        [Required, Url, MaxLength(500)]
        public string AvatarUrl { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
