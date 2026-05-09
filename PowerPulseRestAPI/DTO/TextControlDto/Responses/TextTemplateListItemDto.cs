using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.TextControlDto.Responses
{
    public class TextTemplateListItemDto
    {
        public long Id { get; set; }
        public string Key { get; set; } = null!;
        public TextTemplateChannel Channel { get; set; }
        public bool IsActive { get; set; }
    }
}
