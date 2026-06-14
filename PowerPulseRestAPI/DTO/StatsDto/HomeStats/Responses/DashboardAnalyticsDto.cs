using PowerPulseRestAPI.DTO.ProjectDto.Responses;
using PowerPulseRestAPI.DTO.VehicleDto.Responses;
using PowerPulseRestAPI.DTO.WorkSessionDto.Responses;

namespace PowerPulseRestAPI.DTO.StatsDto.HomeStats.Responses
{
    public class DashboardAnalyticsDto
    {
        public DashboardSummaryDto Summary { get; set; } = new();
        public CurrentMonthActivityDto MonthlyActivity { get; set; } = new();

        public EmployeeWorkSessionsListItemDto? ActiveWorkSession { get; set; }
        public List<EmployeeTaskListItemDto> AssignedTasks { get; set; } = new();
        public VehicleListItemDto? Vehicle { get; set; }
    }
}
