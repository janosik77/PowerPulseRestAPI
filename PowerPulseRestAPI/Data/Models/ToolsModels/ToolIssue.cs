using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.ToolsModels
{


    public class ToolIssue
    {
        public long Id { get; set; }
        public long ToolAssetId { get; set; }
        public long ReportedByUserId { get; set; }
        public ToolIssueType IssueType { get; set; }
        public string Description { get; set; } = null!;
        public GenericStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public ToolAsset? ToolAsset { get; set; }
        public User? ReportedByUser { get; set; }
        public List<ToolIssueAttachment> Attachments { get; set; } = new();

    }

}
