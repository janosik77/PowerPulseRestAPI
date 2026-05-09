using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.WorkSessionDto.Requests
{
    public class CloseWorkSessionDto
    {
        [Required]
        public DateTimeOffset EndedAt { get; set; }

        [MaxLength(2000)]
        public string? Note { get; set; }
    }
}
