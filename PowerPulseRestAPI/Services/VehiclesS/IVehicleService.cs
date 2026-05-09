using PowerPulseRestAPI.DTO.VehicleDto.Requests;
using PowerPulseRestAPI.DTO.VehicleDto.Responses;

namespace PowerPulseRestAPI.Services.VehiclesS
{
    public interface IVehicleService
    {
        Task<VehicleDetailDto> CreateAsync(
           CreateVehicleDto dto,
           CancellationToken cancellationToken = default);

        Task<IReadOnlyList<VehicleListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default);

        Task<VehicleDetailDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task<VehicleListItemDto> GetByEmployeeAsync(
            long employeeId,
            CancellationToken cancellationToken = default);

        Task<VehicleDetailDto> UpdateAsync(
            long id,
            UpdateVehicleDto dto,
            CancellationToken cancellationToken = default);

        Task<VehicleDetailDto> AssignAsync(
            long id,
            CreateVehicleAssignmentDto dto,
            CancellationToken cancellationToken = default);

        Task<VehicleDetailDto> ReturnAsync(
            long id,
            string? note,
            CancellationToken cancellationToken = default);

        Task<VehicleIssueListItemDto> CreateIssueAsync(
            long vehicleId,
            CreateVehicleIssueDto dto,
            long reportedByUserId,
            CancellationToken cancellationToken = default);

        Task<VehicleIssueListItemDto> UpdateIssueAsync(
            long vehicleId,
            long issueId,
            UpdateVehicleIssueDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteIssueAsync(
            long vehicleId,
            long issueId,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default);
    }

}
