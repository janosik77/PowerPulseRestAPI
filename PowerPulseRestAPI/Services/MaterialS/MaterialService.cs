using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.MaterialsModels;
using PowerPulseRestAPI.DTO.MaterialDto.Requests;
using PowerPulseRestAPI.DTO.MaterialDto.Responses;
using PowerPulseRestAPI.DTO.StockDto.Requests;
using PowerPulseRestAPI.DTO.StockDto.Responses;
using PowerPulseRestAPI.Mappers.Materials;
using PowerPulseRestAPI.Mappers.StockNotes;
using PowerPulseRestAPI.Services.MaterialS;
using PowerPulseRestAPI.Services.Uploads;

namespace PowerPulseRestAPI.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly PowerPulseContext _dbContext;
        private readonly IFileStorageService _fileStorageService;

        public MaterialService(PowerPulseContext db, IFileStorageService fileStorageService)
        {
            _dbContext = db;
            _fileStorageService = fileStorageService;
        }

        public async Task<long> CreateCategoryAsync(
            CreateMaterialCategoryDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var exists = await _dbContext.MaterialCategories
                .AnyAsync(x => x.Name == dto.Name, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException($"Material category '{dto.Name}' already exists.");
            }

            var category = new MaterialCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _dbContext.MaterialCategories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return category.Id;
        }

        public async Task UpdateCategoryAsync(
            long id,
            UpdateMaterialCategoryDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var category = await _dbContext.MaterialCategories
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (category is null)
            {
                throw new KeyNotFoundException($"Material category with id '{id}' was not found.");
            }

            var exists = await _dbContext.MaterialCategories
                .AnyAsync(x => x.Id != id && x.Name == dto.Name, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException($"Material category '{dto.Name}' already exists.");
            }

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<long> CreateAsync(
            CreateMaterialDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var categoryExists = await _dbContext.MaterialCategories
                .AnyAsync(x => x.Id == dto.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new KeyNotFoundException($"Material category with id '{dto.CategoryId}' was not found.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Barcode))
            {
                var barcodeExists = await _dbContext.Materials
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Barcode == dto.Barcode, cancellationToken);

                if (barcodeExists)
                {
                    throw new InvalidOperationException($"Material with barcode '{dto.Barcode}' already exists.");
                }
            }

            var now = DateTimeOffset.UtcNow;

            var material = new Material
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Manufacturer = dto.Manufacturer,
                Barcode = dto.Barcode,
                DefaultUnit = dto.DefaultUnit,
                IsActive = dto.IsActive,
                Url = dto.Url,
                Price = dto.Price,
                Currency = dto.Currency,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false,
            };

            _dbContext.Materials.Add(material);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return material.Id;
        }

        public async Task<MaterialDetailsDto> UpdateAsync(
    long id,
    UpdateMaterialDto dto,
    CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var material = await _dbContext.Materials
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (material is null)
            {
                throw new KeyNotFoundException($"Material with id '{id}' was not found.");
            }

            var categoryExists = await _dbContext.MaterialCategories
                .AnyAsync(x => x.Id == dto.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new KeyNotFoundException($"Material category with id '{dto.CategoryId}' was not found.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Barcode))
            {
                var barcodeExists = await _dbContext.Materials
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Id != id && x.Barcode == dto.Barcode, cancellationToken);

                if (barcodeExists)
                {
                    throw new InvalidOperationException($"Material with barcode '{dto.Barcode}' already exists.");
                }
            }

            var oldMaterialUrl = material.Url;

            material.Name = dto.Name;
            material.Description = dto.Description;
            material.CategoryId = dto.CategoryId;
            material.Manufacturer = dto.Manufacturer;
            material.Barcode = dto.Barcode;
            material.DefaultUnit = dto.DefaultUnit;
            material.IsActive = dto.IsActive;
            material.Url = dto.Url;
            material.Price = dto.Price;
            material.Currency = dto.Currency;
            material.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            if (!string.Equals(oldMaterialUrl, material.Url, StringComparison.OrdinalIgnoreCase))
            {
                _fileStorageService.DeleteFileByUrl(oldMaterialUrl);
            }

            var updated = await _dbContext.Materials
                .AsNoTracking()
                .Include(x => x.Category)
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailsDto();
        }

        public async Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var material = await _dbContext.Materials
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (material is null)
            {
                throw new KeyNotFoundException($"Material with id '{id}' was not found.");
            }

            material.IsDeleted = true;
            material.UpdatedAt = DateTimeOffset.UtcNow;
            material.IsActive = false;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateMovementAsync(
            CreateMaterialMovementDto dto,
            long createdByUserId,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var material = await _dbContext.Materials
                .FirstOrDefaultAsync(x => x.Id == dto.MaterialId, cancellationToken);

            if (material is null)
            {
                throw new KeyNotFoundException($"Material with id '{dto.MaterialId}' was not found.");
            }

            var userExists = await _dbContext.Users
                .AnyAsync(x => x.Id == createdByUserId, cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException($"User with id '{createdByUserId}' was not found.");
            }

            if (dto.ProjectId.HasValue)
            {
                var projectExists = await _dbContext.Projects
                    .AnyAsync(x => x.Id == dto.ProjectId.Value, cancellationToken);

                if (!projectExists)
                {
                    throw new KeyNotFoundException($"Project with id '{dto.ProjectId.Value}' was not found.");
                }
            }

            if (dto.Quantity <= 0)
            {
                throw new InvalidOperationException("Quantity must be greater than zero.");
            }

            ValidateMovement(dto);

            var now = DateTimeOffset.UtcNow;

            var movement = new MaterialMovement
            {
                MaterialId = dto.MaterialId,
                MovementType = dto.MovementType,
                ProjectId = dto.ProjectId,
                OperationId = Guid.NewGuid(),
                Quantity = dto.Quantity,
                Unit = dto.Unit,
                Note = dto.Note,
                OccurredAt = now,
                CreatedByUserId = createdByUserId,
                CreatedAt = now
            };

            _dbContext.MaterialMovements.Add(movement);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<MaterialStockDto>> GetStockAsync(
            CancellationToken cancellationToken = default)
        {
            var grouped = await _dbContext.MaterialMovements
                .AsNoTracking()
                .Include(x => x.Material)
                .GroupBy(x => new
                {
                    x.MaterialId,
                    x.Material.Name,
                    x.Material.Manufacturer,
                    x.Material.Url,
                    x.Unit
                })
                .Select(g => new MaterialStockDto
                {
                    MaterialId = g.Key.MaterialId,
                    MaterialName = g.Key.Name,
                    Manufacturer = g.Key.Manufacturer,
                    Url = g.Key.Url,
                    Quantity = g.Sum(x =>
                        x.MovementType == MaterialMovementType.PURCHASE_RECEIPT ? x.Quantity :
                        x.MovementType == MaterialMovementType.RETURN_FROM_PROJECT ? x.Quantity :
                        x.MovementType == MaterialMovementType.WAREHOUSE_ADJUSTMENT_INCREASE ? x.Quantity :
                        x.MovementType == MaterialMovementType.ISSUE_TO_PROJECT ? -x.Quantity :
                        x.MovementType == MaterialMovementType.WAREHOUSE_ADJUSTMENT_DECREASE ? -x.Quantity :
                        0m
                    ),
                    Unit = g.Key.Unit
                })
                .OrderBy(x => x.MaterialName)
                .ToListAsync(cancellationToken);

            return grouped;
        }

        public async Task<IReadOnlyList<ProjectMaterialBalanceDto>> GetProjectBalanceAsync(
            long projectId,
            CancellationToken cancellationToken = default)
        {
            var projectExists = await _dbContext.Projects
                .AnyAsync(x => x.Id == projectId, cancellationToken);

            if (!projectExists)
            {
                throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
            }

            var grouped = await _dbContext.MaterialMovements
                .AsNoTracking()
                .Include(x => x.Material)
                .Where(x => x.ProjectId == projectId)
                .GroupBy(x => new
                {
                    x.MaterialId,
                    x.Material.Name,
                    x.Material.Manufacturer,
                    x.Material.Url,
                    x.Unit
                })
                .Select(g => new ProjectMaterialBalanceDto
                {
                    ProjectId = projectId,
                    MaterialId = g.Key.MaterialId,
                    MaterialName = g.Key.Name,
                    Manufacturer = g.Key.Manufacturer,
                    Url = g.Key.Url,
                    Quantity = g.Sum(x =>
                        x.MovementType == MaterialMovementType.ISSUE_TO_PROJECT ? x.Quantity :
                        x.MovementType == MaterialMovementType.PROJECT_CONSUME_ADJUSTMENT_DECREASE ? x.Quantity :
                        x.MovementType == MaterialMovementType.RETURN_FROM_PROJECT ? -x.Quantity :
                        x.MovementType == MaterialMovementType.PROJECT_CONSUME ? -x.Quantity :
                        0m
                    ),
                    Unit = g.Key.Unit
                })
                .OrderBy(x => x.MaterialName)
                .ToListAsync(cancellationToken);

            return grouped;
        }

        public async Task<IReadOnlyList<ProjectMaterialConsumeDto>> GetProjectConsumptionAsync(
    long projectId,
    CancellationToken cancellationToken = default)
        {
            var projectExists = await _dbContext.Projects
                .AnyAsync(x => x.Id == projectId, cancellationToken);

            if (!projectExists)
            {
                throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
            }

            var grouped = await _dbContext.MaterialMovements
                .AsNoTracking()
                .Where(x =>
                    x.ProjectId == projectId &&
                    (
                        x.MovementType == MaterialMovementType.PROJECT_CONSUME ||
                        x.MovementType == MaterialMovementType.PROJECT_CONSUME_ADJUSTMENT_DECREASE
                    ))
                .GroupBy(x => new
                {
                    x.MaterialId,
                    x.Material.Name,
                    x.Material.Manufacturer,
                    x.Material.Url,
                    x.Unit
                })
                .Select(g => new ProjectMaterialConsumeDto
                {
                    ProjectId = projectId,
                    MaterialId = g.Key.MaterialId,
                    MaterialName = g.Key.Name,
                    Manufacturer = g.Key.Manufacturer,
                    Url = g.Key.Url,

                    ConsumedQuantity = g.Sum(x =>
                        x.MovementType == MaterialMovementType.PROJECT_CONSUME
                            ? x.Quantity
                            : -x.Quantity),

                    Unit = g.Key.Unit
                })
                .Where(x => x.ConsumedQuantity > 0)
                .OrderBy(x => x.MaterialName)
                .ToListAsync(cancellationToken);

            return grouped;
        }

        private static void ValidateMovement(CreateMaterialMovementDto dto)
        {
            switch (dto.MovementType)
            {
                case MaterialMovementType.PURCHASE_RECEIPT:
                case MaterialMovementType.WAREHOUSE_ADJUSTMENT_INCREASE:
                case MaterialMovementType.WAREHOUSE_ADJUSTMENT_DECREASE:
                    if (dto.ProjectId.HasValue)
                    {
                        throw new InvalidOperationException("Selected movement type cannot have ProjectId.");
                    }
                    break;

                case MaterialMovementType.ISSUE_TO_PROJECT:
                case MaterialMovementType.RETURN_FROM_PROJECT:
                case MaterialMovementType.PROJECT_CONSUME:
                    if (!dto.ProjectId.HasValue)
                    {
                        throw new InvalidOperationException("Selected movement type requires ProjectId.");
                    }
                    break;
            }
        }

        public async Task<LowStockNoteDto> CreateLowStockNoteAsync(
            CreateLowStockNoteDto dto,
            long currentEmployeeId,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var employeeExists = await _dbContext.Employees
                .AnyAsync(x => x.Id == currentEmployeeId, cancellationToken);

            if (!employeeExists)
            {
                throw new KeyNotFoundException($"Employee with id '{currentEmployeeId}' was not found.");
            }

            var now = DateTimeOffset.UtcNow;

            var note = new Data.Models.StockModels.LowStockNote
            {
                Priority = dto.Priority,
                Note = dto.Note,
                CreatedByEmployeeId = currentEmployeeId,
                CreatedAt = now,
                UpdatedAt = now
            };

            _dbContext.LowStockNotes.Add(note);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var created = await GetLowStockNoteQuery()
                .FirstAsync(x => x.Id == note.Id, cancellationToken);

            return created.ToDto(currentEmployeeId, false);
        }

        public async Task<LowStockNoteListResponseDto> GetLowStockNotesAsync(
            long currentEmployeeId,
            bool isAdmin,
            CancellationToken cancellationToken = default)
        {
            var employeeExists = await _dbContext.Employees
                .AnyAsync(x => x.Id == currentEmployeeId, cancellationToken);

            if (!employeeExists)
            {
                throw new KeyNotFoundException($"Employee with id '{currentEmployeeId}' was not found.");
            }

            var notes = await GetLowStockNoteQuery()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            var items = notes
                .Select(x => x.ToDto(currentEmployeeId, isAdmin))
                .ToList();

            return new LowStockNoteListResponseDto
            {
                Notes = items,
                Count = items.Count
            };
        }

        public async Task<LowStockNoteDto> GetLowStockNoteByIdAsync(
            long id,
            long currentEmployeeId,
            bool isAdmin,
            CancellationToken cancellationToken = default)
        {
            var note = await GetLowStockNoteQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (note is null)
            {
                throw new KeyNotFoundException($"Low stock note with id '{id}' was not found.");
            }

            return note.ToDto(currentEmployeeId, isAdmin);
        }

        public async Task<LowStockNoteDto> UpdateLowStockNoteAsync(
            long id,
            UpdateLowStockNoteDto dto,
            long currentEmployeeId,
            bool isAdmin,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var note = await _dbContext.LowStockNotes
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (note is null)
            {
                throw new KeyNotFoundException($"Low stock note with id '{id}' was not found.");
            }

            EnsureCanManageLowStockNote(note.CreatedByEmployeeId, currentEmployeeId, isAdmin);

            note.Priority = dto.Priority;
            note.Note = dto.Note;
            note.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var updated = await GetLowStockNoteQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDto(currentEmployeeId, isAdmin);
        }

        public async Task DeleteLowStockNoteAsync(
            long id,
            long currentEmployeeId,
            bool isAdmin,
            CancellationToken cancellationToken = default)
        {
            var note = await _dbContext.LowStockNotes
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (note is null)
            {
                throw new KeyNotFoundException($"Low stock note with id '{id}' was not found.");
            }

            EnsureCanManageLowStockNote(note.CreatedByEmployeeId, currentEmployeeId, isAdmin);

            _dbContext.LowStockNotes.Remove(note);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<Data.Models.StockModels.LowStockNote> GetLowStockNoteQuery()
        {
            return _dbContext.LowStockNotes
                .AsNoTracking()
                .Include(x => x.CreatedByEmployee)
                    .ThenInclude(x => x.Person);
        }

        private static void EnsureCanManageLowStockNote(
            long ownerEmployeeId,
            long currentEmployeeId,
            bool isAdmin)
        {
            var canManage = isAdmin || ownerEmployeeId == currentEmployeeId;

            if (!canManage)
            {
                throw new UnauthorizedAccessException(
                    "You can modify or delete only your own low stock note.");
            }
        }

        public async Task<IEnumerable<MaterialCategorySelectListDto>> GetMaterialCategoriesSelectListAsync()
        {
            return await _dbContext.MaterialCategories
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new MaterialCategorySelectListDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<MaterialDetailsDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var material = await _dbContext.Materials
                .AsNoTracking()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (material is null)
            {
                throw new KeyNotFoundException($"Material with id '{id}' was not found.");
            }

            return material.ToDetailsDto();
        }
    }
}