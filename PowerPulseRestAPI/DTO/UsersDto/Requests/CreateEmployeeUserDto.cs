using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.UsersDto.Requests
{
    public class CreateEmployeeUserDto
    {
        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Login { get; set; } = null!;

        [Required, MinLength(8), MaxLength(200)]
        public string Password { get; set; } = null!;

        [Required]
        [Range(1, long.MaxValue)]
        public long RoleId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
