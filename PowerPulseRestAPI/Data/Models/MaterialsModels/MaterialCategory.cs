namespace PowerPulseRestAPI.Data.Models.MaterialsModels
{
    public class MaterialCategory
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long? ParentId { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public MaterialCategory? Parent { get; set; }
        public List<MaterialCategory> Children { get; set; } = new();
        public List<Material> Materials { get; set; } = new();
    }
}
