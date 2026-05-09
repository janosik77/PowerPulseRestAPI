using PowerPulseRestAPI.Data.Models.TextControlModels;
using PowerPulseRestAPI.DTO.TextControlDto.Responses;

namespace PowerPulseRestAPI.Mappers.TextTemplates
{
    public static class TextTemplateMapper
    {
        public static TextTemplateListItemDto ToListItemDto(this TextTemplate template)
        {
            return new TextTemplateListItemDto
            {
                Id = template.Id,
                Key = template.Key,
                Channel = template.Channel,
                IsActive = template.IsActive
            };
        }

        public static TextTemplateDto ToDto(this TextTemplate template)
        {
            return new TextTemplateDto
            {
                Id = template.Id,
                Key = template.Key,
                Channel = template.Channel,
                TitleTemplate = template.TitleTemplate,
                BodyTemplate = template.BodyTemplate,
                IsActive = template.IsActive,
                CreatedAt = template.CreatedAt,
                UpdatedAt = template.UpdatedAt
            };
        }
    }
}