namespace PowerPulseRestAPI.Data.Models.ToolsModels
{


    public class ToolImage
    {
        public long Id { get; set; }
        public long ToolId { get; set; }
        public string Url { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public string? AltText { get; set; }
        public int SortOrder { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Tool? Tool { get; set; }
    }

}
