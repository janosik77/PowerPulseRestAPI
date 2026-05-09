using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ToolDto.Responses
{
    public class ToolListItemDto
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Url { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(200)]
        public string? Manufacturer { get; set; }

        [MaxLength(200)]
        public string? Model { get; set; }

        [Required]
        [EnumDataType(typeof(ToolStatus))]
        public ToolStatus Status { get; set; }

        [Required]
        [MaxLength(200)]
        public string CategoryName { get; set; } = null!;

        public bool IsAssigned { get; set; }

        public long? AssignedEmployeeId { get; set; }

        [MaxLength(200)]
        public string? AssignedEmployeeName { get; set; }

    }
}
