namespace PowerPulseRestAPI.DTO.MaterialDto.Requests
{
    public class CreateMaterialCategoryDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
