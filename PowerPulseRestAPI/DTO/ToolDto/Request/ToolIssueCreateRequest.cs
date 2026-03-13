using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ToolDto.Request
{
    public sealed class ToolIssueCreateRequest
    {
        public ToolIssueType IssueType { get; set; }
        public string Description { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
    }
}
