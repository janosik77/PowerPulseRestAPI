using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ToolDto.Responses;


public class ToolFilterDto
{
    public string? Search { get; set; }

    public ToolStatus? Status { get; set; }

    public long? EmployeeId { get; set; }

    public string SortBy { get; set; } = "id";

    public string SortOrder { get; set; } = "asc";
}