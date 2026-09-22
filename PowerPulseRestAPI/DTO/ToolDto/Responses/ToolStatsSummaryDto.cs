namespace PowerPulseRestAPI.DTO.ToolDto.Responses;

public class ToolStatsSummaryDto
{
    public int TotalTools { get; set; }

    public int InStockTools { get; set; }

    public int AssignedTools { get; set; }

    public int InServiceTools { get; set; }
}