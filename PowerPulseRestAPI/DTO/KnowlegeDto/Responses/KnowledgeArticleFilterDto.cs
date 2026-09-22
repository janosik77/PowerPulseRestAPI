using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.KnowlegeDto.Responses;

public class KnowledgeArticleFilterDto
{
    public string? Search { get; set; }
    
    public PublishStatus? Status { get; set; }
}