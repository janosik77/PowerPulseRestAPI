using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.KnowlegeDto.Helpers
{
    public class KnowledgeArticleAttachmentDto
    {
        public long Id { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public int SortOrder { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
