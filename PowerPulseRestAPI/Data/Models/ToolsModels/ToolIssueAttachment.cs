namespace PowerPulseRestAPI.Data.Models.ToolsModels
{


    public class ToolIssueAttachment
    {
        public long Id { get; set; }
        public long ToolIssueId { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public ToolIssue? ToolIssue { get; set; }
    }

}
