using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.UsersDto.Requests
{
    public class UpdateEmployeeUserDto
    {
        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Login { get; set; } = null!;

        [MaxLength(200)]
        public string? NewPassword { get; set; }

        [Required, Range(1, long.MaxValue)]
        public long RoleId { get; set; }

        [Required] 
        public bool IsActive { get; set; } = true;
    }
}
