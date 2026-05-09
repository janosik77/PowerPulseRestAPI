using PowerPulseRestAPI.DTO.TextControlDto.Requests;
using PowerPulseRestAPI.DTO.TextControlDto.Responses;

namespace PowerPulseRestAPI.Services.TextTemplateS
{
    public interface ITextTemplateService
    {
        Task<TextTemplateDto> CreateAsync(
            CreateTextTemplateDto dto,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TextTemplateListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default);

        Task<TextTemplateDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task<TextTemplateDto> UpdateAsync(
            long id,
            UpdateTextTemplateDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task<TextTemplateDto> SetActiveAsync(
            long id,
            bool isActive,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TextTemplateOptionDto>> GetOptionsAsync(
            CancellationToken cancellationToken = default);
    }
}
