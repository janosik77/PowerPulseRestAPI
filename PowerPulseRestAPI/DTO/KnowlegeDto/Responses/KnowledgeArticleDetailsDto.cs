using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.KnowlegeDto.Helpers;

namespace PowerPulseRestAPI.DTO.KnowlegeDto.Responses
{
    public class KnowledgeArticleDetailsDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public PublishStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public KnowledgeCategoryDto? Category { get; set; }
        public KnowledgeUserShortDto? CreatedByUser { get; set; }

        public List<KnowledgeArticleAttachmentDto> Attachments { get; set; } = new();

        public int FavoritesCount { get; set; }
        public int ReadsCount { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsRead { get; set; }
    }
}
