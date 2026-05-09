using PowerPulseRestAPI.DTO.PersonDto.Responses;
using PowerPulseRestAPI.DTO.UsersDto.Responses;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.EmployeeDto.Responses
{
    public class EmployeeDetailPrivateDto
    {
        public long Id { get; set; }

        [Required]
        public EmployeePersonDto Person { get; set; } = null!;

        [Required]
        public EmployeeUserDto User { get; set; } = null!;

        [Required]
        public EmployeeEmploymentDto Employment { get; set; } = null!;

        [Required]
        public EmployeeIdentifierDto Identifier { get; set; } = null!;


    }
}
