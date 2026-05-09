using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.PersonDto.Responses
{
    public class EmployeeIdentifierDto
    {
        public long Id { get; set; }

        [Required]
        public string SSN { get; set; } = null!;

    }
}
