using PowerPulseRestAPI.DTO.ToolDto.Requests;
using PowerPulseRestAPI.DTO.ToolDto.Responses;

namespace PowerPulseRestAPI.Services.ToolsS
{
    public interface IToolCategoryService
    {
        Task<long> CreateAsync(
            CreateToolCategoryDto dto,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SelectOptionToolCategoryDto>> GetSelectOptionsAsync(
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default);
    }
}
