using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ProjectDto.Requests
{
    public class CreateProjectNoteDto
    {
        [Required]
        public long ProjectId { get; set; }
        [Required]
        [MaxLength(4000)]
        public string Content { get; set; } = null!;
        [Required]
        public NoteType NoteType { get; set; }
        [Required]
        public long CreatedByEmployeeId { get; set; }

    }
}
