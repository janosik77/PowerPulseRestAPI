using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.PersonDto.Requests
{
    public class CreatePersonIdentifierDto
    {
        [Required, MaxLength(100)]
        public string Value { get; set; } = null!;
    }
}
