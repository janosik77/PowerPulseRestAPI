namespace PowerPulseRestAPI.DTO.ToolDto.Responses;

public class RecentToolAssignmentDto
{
    public long ToolId { get; set; }

    public string ToolName { get; set; } = string.Empty;

    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public ToolAssignmentDto Assignment { get; set; } = new();
}