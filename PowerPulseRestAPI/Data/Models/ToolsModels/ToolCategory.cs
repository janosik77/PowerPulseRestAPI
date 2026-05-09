namespace PowerPulseRestAPI.Data.Models.ToolsModels
{


    public class ToolCategory
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long? ParentId { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public ToolCategory? Parent { get; set; }
        public List<ToolCategory> Children { get; set; } = new();
        public List<Tool> Tools { get; set; } = new();

    }

}
