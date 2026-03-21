using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class CreateProjectAttachmentDto
    {
        public long ProjectId { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public long CreatedByUserId { get; set; }
    }
}
