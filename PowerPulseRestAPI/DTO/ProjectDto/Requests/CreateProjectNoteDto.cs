using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class CreateProjectNoteDto
    {
        public long ProjectId { get; set; }
        public string Content { get; set; } = null!;
        public NoteType NoteType { get; set; }
        public long CreatedByUserId { get; set; }

    }
}
