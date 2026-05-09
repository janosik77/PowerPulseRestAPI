using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.AddresesDto.Requests;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class CreateProjectDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(4000)]
        public string? Description { get; set; }

        [MaxLength(2048)]
        public string? AvatarUrl { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; } = null!;

        [Required]
        public ProjectStatus Status { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long CreatedByEmployeeId { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long CustomerId { get; set; }

        [Range(1, long.MaxValue)]
        public long? ResponsibleEmployeeId { get; set; }

        [Required]
        public CreateAddressDto Address { get; set; } = null!;
    }
}
