using PowerPulseRestAPI.DTO.DictionaryDto.Requests;
using PowerPulseRestAPI.DTO.DictionaryDto.Responses;

namespace PowerPulseRestAPI.BuissnesLogic.DictionaryS;

public interface IDictionaryService
{
    Task<IReadOnlyList<DictionaryDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<DictionaryDto> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default);

    Task<DictionaryOverviewDto> GetOverviewAsync(
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<DictionaryDto> CreateAsync(
        CreateDictionaryDto dto,
        CancellationToken cancellationToken = default);

    Task<DictionaryDto> UpdateAsync(
        string key,
        UpdateDictionaryDto dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string key,
        CancellationToken cancellationToken = default);
}