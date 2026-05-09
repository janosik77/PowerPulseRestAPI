using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.DTO.ToolDto.Requests;
using PowerPulseRestAPI.DTO.ToolDto.Responses;

namespace PowerPulseRestAPI.Services.ToolsS
{
    public interface IToolService
    {
        Task<ToolDetailsDto> CreateAsync(
            CreateToolDto dto,
            long createdByUserId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ToolListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default);

        Task<ToolDetailsDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task<ToolDetailsDto> UpdateAsync(
            long id,
            UpdateToolDto dto,
            CancellationToken cancellationToken = default);

        Task<ToolDetailsDto> AssignAsync(
            string role,
            long employeeId,
            long id,
            AssignToolDto dto,
            long createdByUserId,
            CancellationToken cancellationToken = default);

        Task<ToolDetailsDto> ReturnAsync(
            string role, 
            long employeeId,
            long id,
            ReturnToolDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default);
    }
}
