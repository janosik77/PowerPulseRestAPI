namespace PowerPulseRestAPI.DTO.HomeDto.Responses
{
    public class DashboardEmptyStatesDto
    {
        public DashboardEmptyStateDto Tasks { get; set; } = new();

        public DashboardEmptyStateDto Vehicle { get; set; } = new();

        public DashboardEmptyStateDto ActiveSession { get; set; } = new();
    }
}