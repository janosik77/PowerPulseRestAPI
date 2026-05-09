using PowerPulseRestAPI.DTO.PersonDto.Requests;
using PowerPulseRestAPI.DTO.UsersDto.Requests;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.EmployeeDto.Requests
{
    public class UpdateEmployeeDto
    {
        [Required]
        public UpdateEmployeePersonDto Person { get; set; } = null!;
        [Required]
        public UpdateEmployeeUserDto User { get; set; } = null!;
        [Required]
        public UpdateEmployeeEmploymentDto Employment { get; set; } = null!;
        [Required]
        public UpdatePersonIdentifierDto Identifier { get; set; } = null!;
    }
}
