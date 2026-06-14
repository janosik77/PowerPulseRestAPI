namespace PowerPulseRestAPI.DTO.StatsDto.HomeStats.Responses
{
    public class DashboardSummaryDto
    {
        public int AttendanceDaysThisMonth { get; set; }
        public int AssignedTasksCount { get; set; }
        public string ActiveSessionStatus { get; set; } = string.Empty;
    }
}
