using PowerPulseRestAPI.DTO.WorkSessionDto.Requests;
using PowerPulseRestAPI.DTO.WorkSessionDto.Responses;
using System.Security.Claims;

namespace PowerPulseRestAPI.Services.WorkSessionS
{
    public interface IWorkSessionService
    {
        Task<WorkSessionListItemDto> CreateAsync(
        ClaimsPrincipal user,
        CreateWorkSessionDto dto,
        CancellationToken cancellationToken = default);

        Task<IReadOnlyList<WorkSessionListItemDto>> GetListAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken = default);

        Task<WorkSessionListItemDto> GetByIdAsync(
            ClaimsPrincipal user,
            long id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EmployeeWorkSessionsListItemDto>> GetByEmployeeIdAsync(
            ClaimsPrincipal user,
            long employeeId,
            CancellationToken cancellationToken = default);

        Task<WorkSessionListItemDto> UpdateAsync(
            ClaimsPrincipal user,
            long id,
            UpdateWorkSessionDto dto,
            CancellationToken cancellationToken = default);

        Task<WorkSessionListItemDto> CloseAsync(
            ClaimsPrincipal user,
            long id,
            CloseWorkSessionDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            ClaimsPrincipal user,
            long id,
            CancellationToken cancellationToken = default);
    }
}
