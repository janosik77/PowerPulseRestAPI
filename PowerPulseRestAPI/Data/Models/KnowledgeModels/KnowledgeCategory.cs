namespace PowerPulseRestAPI.Data.Models.KnowledgeModels
{


    public class KnowledgeCategory
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public List<KnowledgeArticle> Articles { get; set; } = new();

    }

}
