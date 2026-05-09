using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.UsersDto.Requests
{
    public class UpdateRoleDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
