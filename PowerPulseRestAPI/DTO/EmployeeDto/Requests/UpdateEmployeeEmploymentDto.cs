using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.EmployeeDto.Requests
{
    public class UpdateEmployeeEmploymentDto
    {

        public DateOnly HireDate { get; set; }

        public DateOnly? TerminatedAt { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        [Required, MaxLength(100)]
        public string JobTitle { get; set; } = null!; 

        [Required, Range(0, 999999.99)]
        public decimal HourlyWage { get; set; }

        [Required, MaxLength(10)]
        public string Currency { get; set; } = "PLN";
        [Required]
        public EmployeeStatus Status { get; set; } 

        [Required, Range(0, 365)]
        public int RemainingVacationDays { get; set; }

        [Required, Range(0, 365)]
        public int VacationDaysPerYear { get; set; }

        [Required, MaxLength(100)]
        public string AccountNumber { get; set; } = null!;
    }
}
