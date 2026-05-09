using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.TextControlModels
{
    public class TextTemplate
    {
        public long Id { get; set; }

        public string Key { get; set; } = null!;
        public TextTemplateChannel Channel { get; set; }

        public string? TitleTemplate { get; set; }
        public string BodyTemplate { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
