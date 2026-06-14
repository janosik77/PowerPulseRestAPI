using PowerPulseRestAPI.DTO.StatsDto.HomeStats.Responses;

namespace PowerPulseRestAPI.BuissnesLogic.HomeS
{
        public interface IHomeService
        {
            Task<DashboardAnalyticsDto> GetDashboardAsync(
                long employeeId,
                CancellationToken cancellationToken);
        }
}