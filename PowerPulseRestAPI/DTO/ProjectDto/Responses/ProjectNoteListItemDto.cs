using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectNoteListItemDto
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Content { get; set; } = null!;
        public NoteType NoteType { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
