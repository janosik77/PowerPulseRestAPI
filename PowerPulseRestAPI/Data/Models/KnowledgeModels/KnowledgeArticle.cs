using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.KnowledgeModels
{


    public class KnowledgeArticle
    {
        public long Id { get; set; }
        public long? CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public KnowledgeArticleType ArticleType { get; set; }
        public SeverityTag SeverityTag { get; set; }
        public PublishStatus Status { get; set; }
        public string? Version { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }
        public long CreatedByUserId { get; set; }
        public long UpdatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public KnowledgeCategory? Category { get; set; }
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
        public List<KnowledgeArticleAttachment> Attachments { get; set; } = new();
        public List<KnowledgeArticleFavorite> Favorites { get; set; } = new();
        public List<KnowledgeArticleRead> Reads { get; set; } = new();

    }

}
