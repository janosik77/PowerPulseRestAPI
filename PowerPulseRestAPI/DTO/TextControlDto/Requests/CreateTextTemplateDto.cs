using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.TextControlDto.Requests
{
    public class CreateTextTemplateDto
    {
        [Required]
        [MaxLength(200)]
        public string Key { get; set; } = null!;

        [Required]
        public TextTemplateChannel Channel { get; set; }

        [MaxLength(500)]
        public string? TitleTemplate { get; set; }

        [Required]
        public string BodyTemplate { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
