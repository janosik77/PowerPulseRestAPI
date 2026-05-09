using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.KnowlegeDto.Requests
{
    public class UpdateKnowledgeArticleAttachmentDto
    {
        public long? Id { get; set; } // null = nowy attachment

        [Required]
        [MaxLength(1000)]
        public string Url { get; set; } = null!;

        [MaxLength(300)]
        public string? Caption { get; set; }

        [Required]
        public AttachmentType AttachmentType { get; set; }

        public int SortOrder { get; set; }
    }
}
