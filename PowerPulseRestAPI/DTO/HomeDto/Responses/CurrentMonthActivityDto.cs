namespace PowerPulseRestAPI.DTO.HomeDto.Responses
{
    public class CurrentMonthActivityDto
    {
        public int Sessions { get; set; }

        public int WorkedDays { get; set; }

        public long TotalMilliseconds { get; set; }

        public double TotalHours { get; set; }

        public double AvgHoursPerDay { get; set; }

        public string TotalDurationLabel { get; set; } = "0h 0m";

        public string AvgHoursPerDayLabel { get; set; } = "0.0h";
    }
}
