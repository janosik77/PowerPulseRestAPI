using PowerPulseRestAPI.DTO.PersonDto.Requests;
using PowerPulseRestAPI.DTO.UsersDto.Requests;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.EmployeeDto.Requests
{
    public class CreateEmployeeDto
    {
        [Required]
        public CreateEmployeePersonDto Person { get; set; } = null!;

        [Required]
        public CreateEmployeeUserDto User { get; set; } = null!;

        [Required]
        public CreateEmployeeEmploymentDto Employment { get; set; } = null!;

        [Required]
        public CreatePersonIdentifierDto Identifier { get; set; } = null!;
    }
}
