using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.KnowlegeDto.Requests
{
    public class UpdateKnowledgeCategoryDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;
    }
}
