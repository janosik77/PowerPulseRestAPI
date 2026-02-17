using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.KnowledgeModels
{


    public class KnowledgeArticleAttachment
    {
        public long Id { get; set; }
        public long ArticleId { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public int SortOrder { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public KnowledgeArticle? Article { get; set; }
    }

}
