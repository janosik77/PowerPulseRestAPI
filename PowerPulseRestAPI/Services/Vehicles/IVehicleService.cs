using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.VehicleDto.Requests;
using PowerPulseRestAPI.DTO.VehicleDto.Responses;

namespace PowerPulseRestAPI.Services.Vehicles
{
    public interface IVehicleService
    {
        Task<IReadOnlyList<VehicleListItemDto>> GetListAsync(CancellationToken ct);
        Task<VehicleDetailsDto?> GetDetailsAsync(long id, CancellationToken ct);
        Task<VehicleLoadDto?> GetLoadAsync(long id, CancellationToken ct);

        Task<VehicleCreatedDto> CreateAsync(CreateVehicleRequest req, CancellationToken ct);
        Task<bool> UpdateAsync(long id, UpdateVehicleRequest req, CancellationToken ct);
        Task<bool> UpdateStatusAsync(long id, VehicleStatus status, CancellationToken ct);

        // "DELETE" jako soft delete
        Task<bool> DeleteAsync(long id, CancellationToken ct);
    }

}
