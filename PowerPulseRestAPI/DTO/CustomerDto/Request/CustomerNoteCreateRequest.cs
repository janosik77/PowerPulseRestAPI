using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.CustomerDto.Request
{
    public class CustomerNoteCreateRequest
    {
        public CustomerNoteType NoteType { get; set; }
        public string Content { get; set; } = null!;
    }
}
