using PowerPulseRestAPI.Data.Models.VehicleModels;
using PowerPulseRestAPI.DTO.VehicleDto.Responses;

namespace PowerPulseRestAPI.Mappers
{
    public static class VehicleMapper
    {
        public static VehicleListItemDto ToListItemDto(Vehicle v) => new()
        {
            Id = v.Id,
            Name = v.Name,
            PlateNumber = v.PlateNumber,
            Make = v.Make,
            Model = v.Model,
            Year = v.Year,
            Status = v.Status,
            CurrentMileage = v.CurrentMileage
        };

        public static VehicleDetailsDto ToDetailsDto(
            Vehicle v,
            VehicleCurrentUserDto? currentUser,
            List<VehicleUsageHistoryItemDto> history
        ) => new()
        {
            Id = v.Id,
            Name = v.Name,
            PlateNumber = v.PlateNumber,
            Vin = v.Vin,
            Make = v.Make,
            Model = v.Model,
            Year = v.Year,
            Status = v.Status,
            CurrentMileage = v.CurrentMileage,
            LastServiceAt = v.LastServiceAt,
            LastServiceMileage = v.LastServiceMileage,
            CurrentUser = currentUser,
            UsageHistory = history
        };
    }

}
