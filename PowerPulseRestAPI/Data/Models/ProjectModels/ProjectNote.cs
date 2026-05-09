using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;


namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class ProjectNote
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Content { get; set; } = null!;
        public NoteType NoteType { get; set; }
        public long CreatedByEmployeeId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Project? Project { get; set; }
        public Employee CreatedByEmployee { get; set; } = null!;
    }

}
