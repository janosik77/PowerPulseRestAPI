using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.ToolsModels
{


    public class ToolIssue
    {
        public long Id { get; set; }
        public long ToolId { get; set; }
        public long ReportedByUserId { get; set; }
        public ToolIssueType IssueType { get; set; }
        public string Description { get; set; } = null!;
        public GenericStatus Status { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Tool Tool { get; set; } = null!;
        public User ReportedByUser { get; set; } = null!;

    }

}
