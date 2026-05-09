using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.TextControlDto.Responses
{
    public class TextTemplateDto
    {
        public long Id { get; set; }
        public string Key { get; set; } = null!;
        public TextTemplateChannel Channel { get; set; }

        public string? TitleTemplate { get; set; }
        public string BodyTemplate { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
