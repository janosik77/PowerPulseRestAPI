using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.MaterialDto.Request;
using PowerPulseRestAPI.DTO.MaterialDto.Response;
using PowerPulseRestAPI.Data.Models.MaterialsModels;
using System.Data;
using PowerPulseRestAPI.Data;

namespace PowerPulseRestAPI.Services.MaterialService
{
    public class MaterialService : IMaterialService
    {
        private readonly PowerPulseContext _db;

        public MaterialService(PowerPulseContext db)
        {
            _db = db;
        }

        public async Task<List<MaterialDto>> GetMaterialsAsync(bool includeArchived, CancellationToken ct)
        {
            var query = _db.Materials
                .AsNoTracking()
                .Include(x => x.Category)
                .AsQueryable();

            if (!includeArchived)
                query = query.Where(x => x.IsActive);

            return await query
                .OrderBy(x => x.Name)
                .Select(x => MapMaterialDto(x))
                .ToListAsync(ct);
        }

        public async Task<MaterialDto?> GetMaterialByIdAsync(long materialId, CancellationToken ct)
        {
            return await _db.Materials
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.Id == materialId)
                .Select(x => MapMaterialDto(x))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<MaterialDto> CreateMaterialAsync(CreateMaterialRequest request, CancellationToken ct)
        {
            Normalize(request);
            ValidateMaterialRequest(request.Sku, request.Name, request.DefaultUnit, request.Url, request.Price, request.Currency);

            var archivedMatch = await _db.Materials
                .FirstOrDefaultAsync(x =>
                    !x.IsActive &&
                    (x.Sku == request.Sku ||
                     (!string.IsNullOrWhiteSpace(request.Barcode) && x.Barcode == request.Barcode)), ct);

            if (archivedMatch != null)
                throw new InvalidOperationException("Material exists in archive. Restore it instead of creating a new one.");

            await EnsureMaterialUniquenessAsync(request.Sku, request.Barcode, null, ct);

            var entity = new Material
            {
                Sku = request.Sku,
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Manufacturer = request.Manufacturer,
                Barcode = request.Barcode,
                DefaultUnit = request.DefaultUnit,
                Url = request.Url,
                Price = request.Price,
                Currency = request.Currency,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            _db.Materials.Add(entity);
            await _db.SaveChangesAsync(ct);

            return await GetMaterialByIdRequiredAsync(entity.Id, ct);
        }

        public async Task<MaterialDto> UpdateMaterialAsync(long materialId, UpdateMaterialRequest request, CancellationToken ct)
        {
            Normalize(request);
            ValidateMaterialRequest(request.Sku, request.Name, request.DefaultUnit, request.Url, request.Price, request.Currency);

            var entity = await _db.Materials.FirstOrDefaultAsync(x => x.Id == materialId, ct)
                ?? throw new InvalidOperationException("Material not found.");

            await EnsureMaterialUniquenessAsync(request.Sku, request.Barcode, materialId, ct);

            entity.Sku = request.Sku;
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.CategoryId = request.CategoryId;
            entity.Manufacturer = request.Manufacturer;
            entity.Barcode = request.Barcode;
            entity.DefaultUnit = request.DefaultUnit;
            entity.Url = request.Url;
            entity.Price = request.Price;
            entity.Currency = request.Currency;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);

            return await GetMaterialByIdRequiredAsync(entity.Id, ct);
        }

        public async Task SoftDeleteMaterialAsync(long materialId, CancellationToken ct)
        {
            var entity = await _db.Materials.FirstOrDefaultAsync(x => x.Id == materialId, ct)
                ?? throw new InvalidOperationException("Material not found.");

            entity.IsActive = false;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
        }

        public async Task RestoreMaterialAsync(long materialId, CancellationToken ct)
        {
            var entity = await _db.Materials.FirstOrDefaultAsync(x => x.Id == materialId, ct)
                ?? throw new InvalidOperationException("Material not found.");

            await EnsureMaterialUniquenessAsync(entity.Sku, entity.Barcode, entity.Id, ct);

            entity.IsActive = true;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
        }

        public async Task HardDeleteMaterialAsync(long materialId, CancellationToken ct)
        {
            var entity = await _db.Materials.FirstOrDefaultAsync(x => x.Id == materialId, ct)
                ?? throw new InvalidOperationException("Material not found.");

            var hasDependencies =
                await _db.MaterialStocks.AnyAsync(x => x.MaterialId == materialId, ct) ||
                await _db.MaterialProjectBalances.AnyAsync(x => x.MaterialId == materialId, ct) ||
                await _db.MaterialMovements.AnyAsync(x => x.MaterialId == materialId, ct) ||
                await _db.MaterialProjectConsumes.AnyAsync(x => x.MaterialId == materialId, ct);

            if (hasDependencies)
                throw new InvalidOperationException("Material cannot be hard deleted because it has related data.");

            _db.Materials.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<MaterialTransferContextDto> GetWarehouseContextAsync(CancellationToken ct)
        {
            var materials = await _db.MaterialStocks
                .AsNoTracking()
                .Include(x => x.Material)
                .Where(x => x.Quantity > 0 && x.Material != null && x.Material.IsActive)
                .OrderBy(x => x.Material!.Name)
                .Select(x => new MaterialBalanceDto
                {
                    MaterialId = x.MaterialId,
                    MaterialName = x.Material!.Name,
                    Sku = x.Material.Sku,
                    Barcode = x.Material.Barcode,
                    Unit = x.Material.DefaultUnit,
                    Quantity = x.Quantity,
                    Price = x.Material.Price,
                    Currency = x.Material.Currency,
                    IsActive = x.Material.IsActive,
                    StorageLocationId = x.StorageLocationId
                })
                .ToListAsync(ct);

            return new MaterialTransferContextDto
            {
                Type = MaterialTransferEndpointType.Warehouse,
                ProjectId = null,
                DisplayName = "Magazyn",
                Materials = materials
            };
        }

        public async Task<MaterialTransferContextDto> GetProjectContextAsync(long projectId, long currentUserId, bool isManager, CancellationToken ct)
        {
            await EnsureProjectAccessAsync(projectId, currentUserId, isManager, ct);

            var project = await _db.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == projectId, ct)
                ?? throw new InvalidOperationException("Project not found.");

            var materials = await _db.MaterialProjectBalances
                .AsNoTracking()
                .Include(x => x.Material)
                .Where(x => x.ProjectId == projectId && x.Quantity > 0 && x.Material != null && x.Material.IsActive)
                .OrderBy(x => x.Material!.Name)
                .Select(x => new MaterialBalanceDto
                {
                    MaterialId = x.MaterialId,
                    MaterialName = x.Material!.Name,
                    Sku = x.Material.Sku,
                    Barcode = x.Material.Barcode,
                    Unit = x.Material.DefaultUnit,
                    Quantity = x.Quantity,
                    Price = x.Material.Price,
                    Currency = x.Material.Currency,
                    IsActive = x.Material.IsActive,
                    StorageLocationId = null
                })
                .ToListAsync(ct);

            return new MaterialTransferContextDto
            {
                Type = MaterialTransferEndpointType.Project,
                ProjectId = projectId,
                DisplayName = $"{project.Code} - {project.Name}",
                Materials = materials
            };
        }

        public async Task<List<MaterialProjectLookupDto>> GetAvailableProjectsAsync(long currentUserId, bool isManager, CancellationToken ct)
        {
            if (isManager)
            {
                return await _db.Projects
                    .AsNoTracking()
                    .OrderBy(x => x.Name)
                    .Select(x => new MaterialProjectLookupDto
                    {
                        ProjectId = x.Id,
                        ProjectCode = x.Code,
                        ProjectName = x.Name
                    })
                    .ToListAsync(ct);
            }

            var employeeId = await GetEmployeeIdForUserAsync(currentUserId, ct);
            if (!employeeId.HasValue)
                return new List<MaterialProjectLookupDto>();

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await _db.ProjectAccesses
                .AsNoTracking()
                .Where(x =>
                    x.EmployeeId == employeeId.Value &&
                    x.IsEnabled &&
                    (!x.ValidFrom.HasValue || x.ValidFrom.Value <= today) &&
                    (!x.ValidTo.HasValue || x.ValidTo.Value >= today))
                .Select(x => x.Project!)
                .OrderBy(x => x.Name)
                .Select(x => new MaterialProjectLookupDto
                {
                    ProjectId = x.Id,
                    ProjectCode = x.Code,
                    ProjectName = x.Name
                })
                .Distinct()
                .ToListAsync(ct);
        }

        public async Task CommitTransferAsync(CommitMaterialTransferRequest request, long currentUserId, bool isManager, CancellationToken ct)
        {
            ValidateTransferRequest(request);

            if (request.SourceType == MaterialTransferEndpointType.Project)
                await EnsureProjectAccessAsync(request.SourceProjectId!.Value, currentUserId, isManager, ct);

            if (request.TargetType == MaterialTransferEndpointType.Project)
                await EnsureProjectAccessAsync(request.TargetProjectId!.Value, currentUserId, isManager, ct);

            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            foreach (var item in request.Items)
            {
                if (request.SourceType == MaterialTransferEndpointType.Warehouse &&
                    request.TargetType == MaterialTransferEndpointType.Project)
                {
                    await TransferWarehouseToProjectAsync(
                        item.MaterialId,
                        item.Quantity,
                        request.TargetProjectId!.Value,
                        request.Note,
                        currentUserId,
                        ct);
                }
                else if (request.SourceType == MaterialTransferEndpointType.Project &&
                         request.TargetType == MaterialTransferEndpointType.Warehouse)
                {
                    await TransferProjectToWarehouseAsync(
                        item.MaterialId,
                        item.Quantity,
                        request.SourceProjectId!.Value,
                        request.Note,
                        currentUserId,
                        ct);
                }
                else if (request.SourceType == MaterialTransferEndpointType.Project &&
                         request.TargetType == MaterialTransferEndpointType.Project)
                {
                    await TransferProjectToProjectAsync(
                        item.MaterialId,
                        item.Quantity,
                        request.SourceProjectId!.Value,
                        request.TargetProjectId!.Value,
                        request.Note,
                        currentUserId,
                        ct);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported transfer type.");
                }
            }

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }

        public async Task<List<ProjectInventoryItemDto>> GetProjectInventoryPreviewAsync(long projectId, long currentUserId, bool isManager, CancellationToken ct)
        {
            await EnsureProjectAccessAsync(projectId, currentUserId, isManager, ct);

            return await _db.MaterialProjectBalances
                .AsNoTracking()
                .Include(x => x.Material)
                .Where(x => x.ProjectId == projectId && x.Quantity > 0 && x.Material != null && x.Material.IsActive)
                .OrderBy(x => x.Material!.Name)
                .Select(x => new ProjectInventoryItemDto
                {
                    MaterialId = x.MaterialId,
                    MaterialName = x.Material!.Name,
                    Sku = x.Material.Sku,
                    Unit = x.Material.DefaultUnit,
                    SystemQuantity = x.Quantity
                })
                .ToListAsync(ct);
        }

        public async Task SubmitProjectInventoryAsync(SubmitProjectInventoryRequest request, long currentUserId, bool isManager, CancellationToken ct)
        {
            if (request.Items == null || request.Items.Count == 0)
                throw new InvalidOperationException("Changed inventory items are required.");

            await EnsureProjectAccessAsync(request.ProjectId, currentUserId, isManager, ct);

            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            foreach (var item in request.Items)
            {
                if (item.ActualQuantity < 0)
                    throw new InvalidOperationException($"Actual quantity cannot be negative for material {item.MaterialId}.");

                var balance = await _db.MaterialProjectBalances
                    .Include(x => x.Material)
                    .FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId && x.MaterialId == item.MaterialId, ct)
                    ?? throw new InvalidOperationException($"Project balance not found for material {item.MaterialId}.");

                var previousQuantity = balance.Quantity;
                var actualQuantity = item.ActualQuantity;
                var consumedQuantity = previousQuantity - actualQuantity;

                if (consumedQuantity > 0)
                {
                    _db.MaterialProjectConsumes.Add(new MaterialProjectConsume
                    {
                        ProjectId = request.ProjectId,
                        MaterialId = item.MaterialId,
                        PreviousQuantity = previousQuantity,
                        ActualQuantity = actualQuantity,
                        ConsumedQuantity = consumedQuantity,
                        Unit = balance.Material!.DefaultUnit,
                        InvoiceId = null,
                        InvoicedAt = null,
                        InventoryBatchId = null,
                        CreatedByUserId = currentUserId,
                        CreatedAt = DateTimeOffset.UtcNow
                    });
                }

                balance.Quantity = actualQuantity;
                balance.UpdatedAt = DateTimeOffset.UtcNow;
            }

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }

        public async Task<List<MaterialProjectConsumeDto>> GetProjectConsumeHistoryAsync(long projectId, long currentUserId, bool isManager, CancellationToken ct)
        {
            await EnsureProjectAccessAsync(projectId, currentUserId, isManager, ct);

            return await _db.MaterialProjectConsumes
                .AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Material)
                .Where(x => x.ProjectId == projectId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new MaterialProjectConsumeDto
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project != null ? x.Project.Name : string.Empty,
                    MaterialId = x.MaterialId,
                    MaterialName = x.Material != null ? x.Material.Name : string.Empty,
                    PreviousQuantity = x.PreviousQuantity,
                    ActualQuantity = x.ActualQuantity,
                    ConsumedQuantity = x.ConsumedQuantity,
                    Unit = x.Unit,
                    InvoiceId = x.InvoiceId,
                    InvoicedAt = x.InvoicedAt,
                    CreatedByUserId = x.CreatedByUserId,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(ct);
        }

        public async Task<List<MaterialMovementDto>> GetMaterialMovementHistoryAsync(long materialId, CancellationToken ct)
        {
            return await _db.MaterialMovements
                .AsNoTracking()
                .Include(x => x.Material)
                .Where(x => x.MaterialId == materialId)
                .OrderByDescending(x => x.OccurredAt)
                .Select(x => new MaterialMovementDto
                {
                    Id = x.Id,
                    MaterialId = x.MaterialId,
                    MaterialName = x.Material != null ? x.Material.Name : string.Empty,
                    MovementType = x.MovementType.ToString(),
                    ProjectId = x.ProjectId,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    Note = x.Note,
                    OccurredAt = x.OccurredAt,
                    CreatedByUserId = x.CreatedByUserId
                })
                .ToListAsync(ct);
        }

        private async Task TransferWarehouseToProjectAsync(
            long materialId,
            decimal quantity,
            long projectId,
            string? note,
            long currentUserId,
            CancellationToken ct)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater than zero.");

            var stock = await _db.MaterialStocks
                .Include(x => x.Material)
                .FirstOrDefaultAsync(x => x.MaterialId == materialId, ct)
                ?? throw new InvalidOperationException($"Warehouse stock not found for material {materialId}.");

            if (!stock.Material!.IsActive)
                throw new InvalidOperationException($"Material {stock.Material.Name} is archived.");

            if (stock.Quantity < quantity)
                throw new InvalidOperationException($"Not enough stock for material {stock.Material.Name}.");

            var projectBalance = await _db.MaterialProjectBalances
                .FirstOrDefaultAsync(x => x.MaterialId == materialId && x.ProjectId == projectId, ct);

            if (projectBalance == null)
            {
                projectBalance = new MaterialProjectBalance
                {
                    MaterialId = materialId,
                    ProjectId = projectId,
                    Quantity = 0,
                    UpdatedAt = DateTimeOffset.UtcNow
                };
                _db.MaterialProjectBalances.Add(projectBalance);
            }

            stock.Quantity -= quantity;
            stock.UpdatedAt = DateTimeOffset.UtcNow;

            projectBalance.Quantity += quantity;
            projectBalance.UpdatedAt = DateTimeOffset.UtcNow;

            _db.MaterialMovements.Add(new MaterialMovement
            {
                MaterialId = materialId,
                MovementType = MaterialMovementType.ISSUE_TO_PROJECT,
                ProjectId = projectId,
                Quantity = quantity,
                Unit = stock.Material.DefaultUnit,
                Note = note,
                OccurredAt = DateTimeOffset.UtcNow,
                CreatedByUserId = currentUserId,
                CreatedAt = DateTimeOffset.UtcNow
            });
        }

        private async Task TransferProjectToWarehouseAsync(
            long materialId,
            decimal quantity,
            long projectId,
            string? note,
            long currentUserId,
            CancellationToken ct)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater than zero.");

            var projectBalance = await _db.MaterialProjectBalances
                .Include(x => x.Material)
                .FirstOrDefaultAsync(x => x.MaterialId == materialId && x.ProjectId == projectId, ct)
                ?? throw new InvalidOperationException($"Project balance not found for material {materialId}.");

            if (!projectBalance.Material!.IsActive)
                throw new InvalidOperationException($"Material {projectBalance.Material.Name} is archived.");

            if (projectBalance.Quantity < quantity)
                throw new InvalidOperationException($"Not enough quantity on project for material {projectBalance.Material.Name}.");

            var stock = await _db.MaterialStocks
                .FirstOrDefaultAsync(x => x.MaterialId == materialId, ct);

            if (stock == null)
            {
                throw new InvalidOperationException($"Warehouse stock configuration not found for material {materialId}.");
            }

            projectBalance.Quantity -= quantity;
            projectBalance.UpdatedAt = DateTimeOffset.UtcNow;

            stock.Quantity += quantity;
            stock.UpdatedAt = DateTimeOffset.UtcNow;

            _db.MaterialMovements.Add(new MaterialMovement
            {
                MaterialId = materialId,
                MovementType = MaterialMovementType.RETURN_FROM_PROJECT,
                ProjectId = projectId,
                Quantity = quantity,
                Unit = projectBalance.Material.DefaultUnit,
                Note = note,
                OccurredAt = DateTimeOffset.UtcNow,
                CreatedByUserId = currentUserId,
                CreatedAt = DateTimeOffset.UtcNow
            });
        }

        private async Task TransferProjectToProjectAsync(
            long materialId,
            decimal quantity,
            long sourceProjectId,
            long targetProjectId,
            string? note,
            long currentUserId,
            CancellationToken ct)
        {
            if (sourceProjectId == targetProjectId)
                throw new InvalidOperationException("Source and target project cannot be the same.");

            await TransferProjectToWarehouseAsync(materialId, quantity, sourceProjectId, note, currentUserId, ct);
            await TransferWarehouseToProjectAsync(materialId, quantity, targetProjectId, note, currentUserId, ct);
        }

        private void ValidateTransferRequest(CommitMaterialTransferRequest request)
        {
            if (request.Items == null || request.Items.Count == 0)
                throw new InvalidOperationException("Transfer items are required.");

            if (request.SourceType == MaterialTransferEndpointType.Warehouse &&
                request.TargetType == MaterialTransferEndpointType.Warehouse)
            {
                throw new InvalidOperationException("Warehouse to warehouse transfer is not supported.");
            }

            if (request.SourceType == MaterialTransferEndpointType.Project && !request.SourceProjectId.HasValue)
                throw new InvalidOperationException("Source project is required.");

            if (request.TargetType == MaterialTransferEndpointType.Project && !request.TargetProjectId.HasValue)
                throw new InvalidOperationException("Target project is required.");

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    throw new InvalidOperationException($"Quantity must be greater than zero for material {item.MaterialId}.");
            }
        }

        private async Task<long?> GetEmployeeIdForUserAsync(long userId, CancellationToken ct)
        {
            return await _db.Employees
                .Where(x => x.Person != null && x.Person.UserId == userId)
                .Select(x => (long?)x.Id)
                .FirstOrDefaultAsync(ct);
        }

        private async Task EnsureProjectAccessAsync(long projectId, long currentUserId, bool isManager, CancellationToken ct)
        {
            if (isManager)
                return;

            var employeeId = await GetEmployeeIdForUserAsync(currentUserId, ct);
            if (!employeeId.HasValue)
                throw new UnauthorizedAccessException("User is not linked to an employee.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var hasAccess = await _db.ProjectAccesses.AnyAsync(x =>
                x.ProjectId == projectId &&
                x.EmployeeId == employeeId.Value &&
                x.IsEnabled &&
                (!x.ValidFrom.HasValue || x.ValidFrom.Value <= today) &&
                (!x.ValidTo.HasValue || x.ValidTo.Value >= today),
                ct);

            if (!hasAccess)
                throw new UnauthorizedAccessException("You do not have access to this project.");
        }

        private async Task EnsureMaterialUniquenessAsync(string sku, string? barcode, long? excludeId, CancellationToken ct)
        {
            var skuExists = await _db.Materials.AnyAsync(x =>
                x.Sku == sku &&
                (!excludeId.HasValue || x.Id != excludeId.Value), ct);

            if (skuExists)
                throw new InvalidOperationException("Material with this SKU already exists.");

            if (!string.IsNullOrWhiteSpace(barcode))
            {
                var barcodeExists = await _db.Materials.AnyAsync(x =>
                    x.Barcode == barcode &&
                    (!excludeId.HasValue || x.Id != excludeId.Value), ct);

                if (barcodeExists)
                    throw new InvalidOperationException("Material with this barcode already exists.");
            }
        }

        private static void Normalize(CreateMaterialRequest request)
        {
            request.Sku = request.Sku.Trim();
            request.Name = request.Name.Trim();
            request.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
            request.Manufacturer = string.IsNullOrWhiteSpace(request.Manufacturer) ? null : request.Manufacturer.Trim();
            request.Barcode = string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode.Trim();
            request.DefaultUnit = request.DefaultUnit.Trim();
            request.Url = request.Url.Trim();
            request.Currency = request.Currency.Trim().ToUpperInvariant();
        }

        private static void Normalize(UpdateMaterialRequest request)
        {
            request.Sku = request.Sku.Trim();
            request.Name = request.Name.Trim();
            request.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
            request.Manufacturer = string.IsNullOrWhiteSpace(request.Manufacturer) ? null : request.Manufacturer.Trim();
            request.Barcode = string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode.Trim();
            request.DefaultUnit = request.DefaultUnit.Trim();
            request.Url = request.Url.Trim();
            request.Currency = request.Currency.Trim().ToUpperInvariant();
        }

        private static void ValidateMaterialRequest(string sku, string name, string unit, string url, decimal price, string currency)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new InvalidOperationException("SKU is required.");

            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("Name is required.");

            if (string.IsNullOrWhiteSpace(unit))
                throw new InvalidOperationException("Default unit is required.");

            if (string.IsNullOrWhiteSpace(url))
                throw new InvalidOperationException("Url is required.");

            if (price < 0)
                throw new InvalidOperationException("Price cannot be negative.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new InvalidOperationException("Currency is required.");
        }

        private static MaterialDto MapMaterialDto(Material x)
        {
            return new MaterialDto
            {
                Id = x.Id,
                Sku = x.Sku,
                Name = x.Name,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CategoryName = x.Category != null ? x.Category.Name : null,
                Manufacturer = x.Manufacturer,
                Barcode = x.Barcode,
                DefaultUnit = x.DefaultUnit,
                IsActive = x.IsActive,
                Url = x.Url,
                Price = x.Price,
                Currency = x.Currency,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            };
        }

        private async Task<MaterialDto> GetMaterialByIdRequiredAsync(long materialId, CancellationToken ct)
        {
            return await _db.Materials
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.Id == materialId)
                .Select(x => MapMaterialDto(x))
                .FirstAsync(ct);
        }
    }
}
