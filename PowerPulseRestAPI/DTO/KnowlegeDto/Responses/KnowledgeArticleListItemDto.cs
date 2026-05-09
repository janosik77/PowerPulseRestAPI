using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.KnowlegeDto.Helpers;

namespace PowerPulseRestAPI.DTO.KnowlegeDto.Responses
{
    public class KnowledgeArticleListItemDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public PublishStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public KnowledgeCategoryDto? Category { get; set; }
        public KnowledgeUserShortDto? CreatedByUser { get; set; }

        public int AttachmentsCount { get; set; }
        public int FavoritesCount { get; set; }
        public int ReadsCount { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsRead { get; set; }
    }
}
