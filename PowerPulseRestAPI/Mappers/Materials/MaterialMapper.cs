using PowerPulseRestAPI.Data.Models.MaterialsModels;
using PowerPulseRestAPI.DTO.MaterialDto.Responses;

namespace PowerPulseRestAPI.Mappers.Materials
{
    public static class MaterialMapper
    {
        public static MaterialDetailsDto ToDetailsDto(this Material material)
        {
            return new MaterialDetailsDto
            {
                Id = material.Id,
                Name = material.Name,
                Description = material.Description,
                CategoryId = material.CategoryId,
                CategoryName = material.Category?.Name,
                Manufacturer = material.Manufacturer,
                Barcode = material.Barcode,
                DefaultUnit = material.DefaultUnit,
                IsActive = material.IsActive,
                Url = material.Url,
                Price = material.Price,
                Currency = material.Currency,
                CreatedAt = material.CreatedAt,
                UpdatedAt = material.UpdatedAt
            };
        }
    }
}
