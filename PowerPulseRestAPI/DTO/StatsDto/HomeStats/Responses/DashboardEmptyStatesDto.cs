namespace PowerPulseRestAPI.DTO.StatsDto.HomeStats.Responses
{
    public class DashboardEmptyStatesDto
    {
        public DashboardEmptyStateDto Tasks { get; set; } = new();

        public DashboardEmptyStateDto Vehicle { get; set; } = new();

        public DashboardEmptyStateDto ActiveSession { get; set; } = new();
    }
}