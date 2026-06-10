namespace PowerPulseRestAPI.DTO.StatsDto.HomeStats.Responses
{
    public class CurrentMonthActivityDto
    {
        public int Sessions { get; set; }
        public int WorkedDays { get; set; }

        public long TotalMilliseconds { get; set; }
        public double TotalHours { get; set; }
        public double AvgHoursPerDay { get; set; }
    }
}
