using PowerPulseRestAPI.DTO.AnalyticsDto.Requests;
using PowerPulseRestAPI.DTO.AnalyticsDto.Responses;

namespace PowerPulseRestAPI.BuissnesLogic.AnalyticsS;

public interface IAnalyticsService
{
    Task<AnalyticsOverviewDto> GetOverviewAsync(
        AnalyticsFilterDto filter,
        CancellationToken cancellationToken = default
    );

    Task<AnalyticsFilterOptionsDto> GetFilterOptionsAsync(
        CancellationToken cancellationToken = default);
}