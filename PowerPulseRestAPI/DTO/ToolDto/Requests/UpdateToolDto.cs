using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ToolDto.Requests
{
    public class UpdateToolDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(4000)]
        public string? Description { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long CategoryId { get; set; }

        [MaxLength(200)]
        public string? Manufacturer { get; set; }

        [MaxLength(200)]
        public string? Model { get; set; }

        [MaxLength(200)]
        public string? SerialNumber { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Url { get; set; } = null!;

        [Required]
        [EnumDataType(typeof(ToolCondition))]
        public ToolCondition Condition { get; set; }

        public DateOnly? PurchaseDate { get; set; }

        public bool IsActive { get; set; }
    }
}
