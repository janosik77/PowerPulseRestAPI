namespace PowerPulseRestAPI.DTO.HomeDto.Responses
{
    public class DashboardSummaryDto
    {
        public long CurrentDayWorkTimeMilliseconds { get; set; }
        public string CurrentDayWorkTimeLabel { get; set; } = "0h 0m";
        public int AttendanceDaysThisMonth { get; set; }
        public int AssignedTasksCount { get; set; }
        public string ActiveSessionStatus { get; set; } = "Not started";
    }
}
