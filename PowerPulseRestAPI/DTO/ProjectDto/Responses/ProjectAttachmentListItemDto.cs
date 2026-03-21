using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectAttachmentListItemDto
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
