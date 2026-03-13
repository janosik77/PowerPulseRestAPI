using PowerPulseRestAPI.DTO.ToolDto.Request;
using PowerPulseRestAPI.DTO.ToolDto.Response;

namespace PowerPulseRestAPI.Services.Tools
{
    public interface IToolService
    {
        Task<IReadOnlyList<ToolListItemDto>> GetListAsync(ToolListQuery query, CancellationToken ct);
        Task<IReadOnlyList<ToolHoldingDto>> GetHoldingsAsync(ToolHoldingsQuery query, long userId, bool isManager, CancellationToken ct);

        Task<long> CreateAsync(ToolCreateRequest req, long managerUserId, CancellationToken ct);
        Task<bool> UpdateAsync(long toolId, ToolUpdateRequest req, long managerUserId, CancellationToken ct);
        Task<bool> SoftDeleteAsync(long toolId, long managerUserId, CancellationToken ct);

        Task<ToolOperationResultDto> AssignAsync(ToolAssignRequest req, long userId, bool isManager, CancellationToken ct);
        Task<ToolOperationResultDto> ReturnToStorageAsync(long toolId, ToolReturnToStorageRequest req, long userId, bool isManager, CancellationToken ct);

        Task<long> ReportIssueAsync(long toolId, ToolIssueCreateRequest req, long userId, CancellationToken ct);
    }
}
