namespace PowerPulseRestAPI.DTO.MaterialDto.Requests
{
    public class UpdateMaterialCategoryDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
