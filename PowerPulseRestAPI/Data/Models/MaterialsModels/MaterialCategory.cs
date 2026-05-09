namespace PowerPulseRestAPI.Data.Models.MaterialsModels
{
    public class MaterialCategory
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public List<Material> Materials { get; set; } = new();
    }
}
