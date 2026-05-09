using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.KnowlegeDto.Requests
{
    public class CreateKnowledgeCategoryDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;
    }
}
