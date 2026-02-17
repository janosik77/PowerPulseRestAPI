using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.KnowledgeModels
{


    public class KnowledgeArticleFavorite
    {
        public long Id { get; set; }
        public long ArticleId { get; set; }
        public long UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public KnowledgeArticle? Article { get; set; }
        public User? User { get; set; }
    }

}
