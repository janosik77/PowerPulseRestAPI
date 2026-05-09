using PowerPulseRestAPI.DTO.MaterialDto.Requests;
using PowerPulseRestAPI.DTO.MaterialDto.Responses;
using PowerPulseRestAPI.DTO.StockDto.Requests;
using PowerPulseRestAPI.DTO.StockDto.Responses;

namespace PowerPulseRestAPI.Services.MaterialS
{
    public interface IMaterialService
    {
        Task<long> CreateCategoryAsync(
           CreateMaterialCategoryDto dto,
           CancellationToken cancellationToken = default);

        Task UpdateCategoryAsync(
            long id,
            UpdateMaterialCategoryDto dto,
            CancellationToken cancellationToken = default);

        Task<long> CreateAsync(
            CreateMaterialDto dto,
            CancellationToken cancellationToken = default);

        Task<MaterialDetailsDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task <MaterialDetailsDto>UpdateAsync(
            long id,
            UpdateMaterialDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task CreateMovementAsync(
            CreateMaterialMovementDto dto,
            long createdByUserId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MaterialStockDto>> GetStockAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProjectMaterialBalanceDto>> GetProjectBalanceAsync(
            long projectId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProjectMaterialConsumeDto>> GetProjectConsumptionAsync(
            long projectId,
            CancellationToken cancellationToken = default);

        Task<LowStockNoteDto> CreateLowStockNoteAsync(
            CreateLowStockNoteDto dto,
            long currentEmployeeId,
            CancellationToken cancellationToken = default);

        Task<LowStockNoteListResponseDto> GetLowStockNotesAsync(
            long currentEmployeeId,
            bool isAdmin,
            CancellationToken cancellationToken = default);

        Task<LowStockNoteDto> GetLowStockNoteByIdAsync(
            long id,
            long currentEmployeeId,
            bool isAdmin,
            CancellationToken cancellationToken = default);

        Task<LowStockNoteDto> UpdateLowStockNoteAsync(
            long id,
            UpdateLowStockNoteDto dto,
            long currentEmployeeId,
            bool isAdmin,
            CancellationToken cancellationToken = default);

        Task DeleteLowStockNoteAsync(
            long id,
            long currentEmployeeId,
            bool isAdmin,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<MaterialCategorySelectListDto>> GetMaterialCategoriesSelectListAsync();
    }
}
