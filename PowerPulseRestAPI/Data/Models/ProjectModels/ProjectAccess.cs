using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class ProjectAccess
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Project Project { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
    }

}
