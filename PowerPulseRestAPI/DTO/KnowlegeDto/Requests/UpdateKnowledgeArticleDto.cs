using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.KnowlegeDto.Requests
{
    public class UpdateKnowledgeArticleDto
    {
        public long? CategoryId { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public PublishStatus Status { get; set; }

        public List<UpdateKnowledgeArticleAttachmentDto> Attachments { get; set; } = new();
    }
}
