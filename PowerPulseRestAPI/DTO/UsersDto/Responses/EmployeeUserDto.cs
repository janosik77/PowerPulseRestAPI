using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.UsersDto.Responses
{
    public class EmployeeUserDto
    {
        public long Id { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Login { get; set; } = null!;

        [Required]
        [Range(1, long.MaxValue)]
        public long RoleId { get; set; }

        [Required, MaxLength(100)]
        public string RoleName { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTimeOffset? LastPasswordUpdate { get; set; }

        public DateTimeOffset? LastLoginAt { get; set; }
    }
}
