namespace PowerPulseRestAPI.Data.Models.KnowledgeModels
{


    public class KnowledgeCategory
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long? ParentId { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public KnowledgeCategory? Parent { get; set; }
        public List<KnowledgeCategory> Children { get; set; } = new();
        public List<KnowledgeArticle> Articles { get; set; } = new();

    }

}
