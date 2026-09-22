namespace PowerPulseRestAPI.DTO.MaterialDto.Responses;

public class MaterialStockFilterDto
{
    public string? Search { get; set; }

    public string SortBy { get; set; } = "materialName";

    public string SortOrder { get; set; } = "asc";
}