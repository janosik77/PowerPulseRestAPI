using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Models.UsersModels
{


    public class WorkSession
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long ProjectId { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
        public WorkSessionStatus Status { get; set; }
        public string? Note { get; set; }

        public long? InvoiceId { get; set; }
        public DateTimeOffset? InvoicedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public long StartedByUserId { get; set; }

        public Invoice? Invoice { get; set; }
        public Employee Employee { get; set; } = null!;
        public Project Project { get; set; } = null!;
        public User StartedByUser { get; set; } = null!;
        public List<ProjectNote> Notes { get; set; } = new();
        public List<ProjectAttachment> Attachments { get; set; } = new();
    }

}
