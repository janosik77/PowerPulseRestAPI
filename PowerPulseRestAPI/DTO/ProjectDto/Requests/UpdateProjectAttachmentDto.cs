using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class UpdateProjectAttachmentDto
    {
        [MaxLength(2048)]
        public string? Url { get; set; }
        [MaxLength(500)]
        public string? Caption { get; set; }
        public AttachmentType? AttachmentType { get; set; }
    }
}
