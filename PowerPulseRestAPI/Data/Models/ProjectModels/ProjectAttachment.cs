using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class ProjectAttachment
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public long CreatedByEmployeeId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Project Project { get; set; } = null!;
        public Employee CreatedByEmployee { get; set; } = null!;
    }

}
