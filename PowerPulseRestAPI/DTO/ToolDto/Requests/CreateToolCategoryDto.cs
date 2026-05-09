using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ToolDto.Requests
{
    public class CreateToolCategoryDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public long? ParentId { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }
    }
}
