using PowerPulseRestAPI.DTO.MaterialDto.Request;
using PowerPulseRestAPI.DTO.MaterialDto.Response;

namespace PowerPulseRestAPI.Services.MaterialService
{
    public interface IMaterialService
    {
        Task<List<MaterialDto>> GetMaterialsAsync(bool includeArchived, CancellationToken ct);
        Task<MaterialDto?> GetMaterialByIdAsync(long materialId, CancellationToken ct);

        Task<MaterialDto> CreateMaterialAsync(CreateMaterialRequest request, CancellationToken ct);
        Task<MaterialDto> UpdateMaterialAsync(long materialId, UpdateMaterialRequest request, CancellationToken ct);

        Task SoftDeleteMaterialAsync(long materialId, CancellationToken ct);
        Task RestoreMaterialAsync(long materialId, CancellationToken ct);
        Task HardDeleteMaterialAsync(long materialId, CancellationToken ct);

        Task<MaterialTransferContextDto> GetWarehouseContextAsync(CancellationToken ct);
        Task<MaterialTransferContextDto> GetProjectContextAsync(long projectId, long currentUserId, bool isManager, CancellationToken ct);
        Task<List<MaterialProjectLookupDto>> GetAvailableProjectsAsync(long currentUserId, bool isManager, CancellationToken ct);

        Task CommitTransferAsync(CommitMaterialTransferRequest request, long currentUserId, bool isManager, CancellationToken ct);

        Task<List<ProjectInventoryItemDto>> GetProjectInventoryPreviewAsync(long projectId, long currentUserId, bool isManager, CancellationToken ct);
        Task SubmitProjectInventoryAsync(SubmitProjectInventoryRequest request, long currentUserId, bool isManager, CancellationToken ct);

        Task<List<MaterialProjectConsumeDto>> GetProjectConsumeHistoryAsync(long projectId, long currentUserId, bool isManager, CancellationToken ct);
        Task<List<MaterialMovementDto>> GetMaterialMovementHistoryAsync(long materialId, CancellationToken ct);
    }
}
