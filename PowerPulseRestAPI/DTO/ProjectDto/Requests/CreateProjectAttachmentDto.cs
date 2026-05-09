using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class CreateProjectAttachmentDto
    {
        [Required]
        public long ProjectId { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Url { get; set; } = null!;
        [MaxLength(500)]
        public string? Caption { get; set; }
        [Required]
        public AttachmentType AttachmentType { get; set; }
        [Required]
        public long CreatedByEmployeeId { get; set; }
    }
}
