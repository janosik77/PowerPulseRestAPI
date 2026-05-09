using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class UpdateProjectNoteDto
    {
        public string? Content { get; set; } = null!;
        public NoteType? NoteType { get; set; }
    }
}
