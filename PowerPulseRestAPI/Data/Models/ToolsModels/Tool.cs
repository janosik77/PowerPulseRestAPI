using PowerPulseRestAPI.Data.Enums;


namespace PowerPulseRestAPI.Data.Models.ToolsModels
{


    public class Tool
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long? CategoryId { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public string Url { get; set; } = null!;
        public ToolCondition Condition { get; set; }
        public ToolStatus Status { get; set; }
        public DateOnly? PurchaseDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public ToolCategory Category { get; set; } = null!;
        public List<ToolAssignment> Assignments { get; set; } = new();
    }

}
