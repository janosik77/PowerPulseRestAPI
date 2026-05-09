using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.AddresesDto.Requests;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class UpdateProjectDto
    {
        [Range(1, long.MaxValue)]
        public long? CustomerId { get; set; }

        [MaxLength(50)]
        public string? Code { get; set; }

        [MaxLength(200)]
        public string? Name { get; set; }

        [MaxLength(2048)]
        public string? AvatarUrl { get; set; }

        [MaxLength(4000)]
        public string? Description { get; set; }

        public ProjectStatus? Status { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        [Range(1, long.MaxValue)]
        public long? ResponsibleEmployeeId { get; set; }

        public UpdateAddressDto? Address { get; set; }
    }
}
