namespace PowerPulseRestAPI.DTO.KnowlegeDto.Responses;

public class KnowledgeStatsSummaryDto
{
    public int TotalArticles { get; set; }
    public int PublishedArticles { get; set; }
    public int DraftArticles { get; set; }
    public int ArchivedArticles { get; set; }

    public int VisibleArticles { get; set; }
}