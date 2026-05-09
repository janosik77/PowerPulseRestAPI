using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ToolDto.Responses
{
    public class SelectOptionToolCategoryDto
    {
        [Required]
        public long Value { get; set; }
        [Required]
        public string Label { get; set; } = null!;
    }
}
