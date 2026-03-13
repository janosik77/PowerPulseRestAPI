using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.CustomerDto.Response
{
    public sealed class CustomerNoteDto
    {
        public long Id { get; set; }
        public CustomerNoteType NoteType { get; set; }
        public string Content { get; set; } = null!;
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
