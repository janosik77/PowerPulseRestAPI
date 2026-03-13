// Services/Tools/ToolService.cs
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.DTO.ToolDto.Request;
using PowerPulseRestAPI.DTO.ToolDto.Response;

namespace PowerPulseRestAPI.Services.Tools
{
    public sealed class ToolService : IToolService
    {
        private readonly PowerPulseContext _db;

        public ToolService(PowerPulseContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<ToolListItemDto>> GetListAsync(ToolListQuery query, CancellationToken ct)
        {
            var q = _db.Set<Tool>().AsNoTracking();

            if (query.IsActive.HasValue)
                q = q.Where(x => x.IsActive == query.IsActive.Value);

            if (query.CategoryId.HasValue)
                q = q.Where(x => x.CategoryId == query.CategoryId.Value);

            if (!string.IsNullOrWhiteSpace(query.Q))
            {
                var s = query.Q.Trim();
                q = q.Where(x =>
                    x.Name.Contains(s) ||
                    (x.Sku != null && x.Sku.Contains(s)) ||
                    (x.Barcode != null && x.Barcode.Contains(s)) ||
                    (x.SerialNumber != null && x.SerialNumber.Contains(s)) ||
                    (x.Manufacturer != null && x.Manufacturer.Contains(s)) ||
                    (x.Model != null && x.Model.Contains(s)));
            }

            return await q
                .OrderBy(x => x.Name)
                .Skip(query.Skip)
                .Take(query.Take)
                .Select(x => new ToolListItemDto
                {
                    Id = x.Id,
                    Sku = x.Sku,
                    Name = x.Name,
                    Description = x.Description,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category != null ? x.Category.Name : null,
                    Manufacturer = x.Manufacturer,
                    Model = x.Model,
                    Barcode = x.Barcode,
                    SerialNumber = x.SerialNumber,
                    Url = x.Url,
                    Condition = x.Condition,
                    Status = x.Status,
                    PurchaseDate = x.PurchaseDate,
                    IsActive = x.IsActive,
                    CurrentStorageLocationId = x.ToolAssetStock != null ? x.ToolAssetStock.StorageLocationId : null,
                    CurrentTargetType = x.ToolAssetStock != null
                        ? ToolTransferTargetType.STORAGE
                        : x.Assignments
                            .Where(a => a.ReturnedAt == null)
                            .OrderByDescending(a => a.AssignedAt)
                            .Select(a => (ToolTransferTargetType?)a.TargetType)
                            .FirstOrDefault(),
                    CurrentTargetId = x.ToolAssetStock != null
                        ? x.ToolAssetStock.StorageLocationId
                        : x.Assignments
                            .Where(a => a.ReturnedAt == null)
                            .OrderByDescending(a => a.AssignedAt)
                            .Select(a => (long?)a.TargetId)
                            .FirstOrDefault()
                })
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<ToolHoldingDto>> GetHoldingsAsync(
            ToolHoldingsQuery query,
            long userId,
            bool isManager,
            CancellationToken ct)
        {
            if (query.TargetId <= 0)
                throw new InvalidOperationException("Invalid TargetId.");

            await EnsureTargetAccessAsync(userId, isManager, query.TargetType, query.TargetId, ct);

            IQueryable<Tool> q = query.TargetType switch
            {
                ToolTransferTargetType.STORAGE => _db.Set<Tool>()
                    .AsNoTracking()
                    .Where(x => x.ToolAssetStock != null && x.ToolAssetStock.StorageLocationId == query.TargetId),

                ToolTransferTargetType.EMPLOYEE => _db.Set<Tool>()
                    .AsNoTracking()
                    .Where(x => x.Assignments.Any(a => a.ReturnedAt == null && a.TargetType == ToolTransferTargetType.EMPLOYEE && a.TargetId == query.TargetId)),

                _ => throw new InvalidOperationException("Unsupported target type.")
            };

            if (!string.IsNullOrWhiteSpace(query.Q))
            {
                var s = query.Q.Trim();
                q = q.Where(x =>
                    x.Name.Contains(s) ||
                    (x.Sku != null && x.Sku.Contains(s)) ||
                    (x.Barcode != null && x.Barcode.Contains(s)) ||
                    (x.SerialNumber != null && x.SerialNumber.Contains(s)));
            }

            return await q
                .OrderByDescending(x => x.UpdatedAt)
                .Skip(query.Skip)
                .Take(query.Take)
                .Select(x => new ToolHoldingDto
                {
                    ToolId = x.Id,
                    Sku = x.Sku,
                    Name = x.Name,
                    SerialNumber = x.SerialNumber,
                    Barcode = x.Barcode,
                    Url = x.Url,
                    Condition = x.Condition,
                    Status = x.Status,
                    CurrentTargetType = x.ToolAssetStock != null
                        ? ToolTransferTargetType.STORAGE
                        : x.Assignments
                            .Where(a => a.ReturnedAt == null)
                            .OrderByDescending(a => a.AssignedAt)
                            .Select(a => a.TargetType)
                            .FirstOrDefault(),
                    CurrentTargetId = x.ToolAssetStock != null
                        ? x.ToolAssetStock.StorageLocationId
                        : x.Assignments
                            .Where(a => a.ReturnedAt == null)
                            .OrderByDescending(a => a.AssignedAt)
                            .Select(a => a.TargetId)
                            .FirstOrDefault()
                })
                .ToListAsync(ct);
        }

        public async Task<long> CreateAsync(ToolCreateRequest req, long managerUserId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                throw new InvalidOperationException("Name is required.");

            if (string.IsNullOrWhiteSpace(req.Url))
                throw new InvalidOperationException("Url is required.");

            var sku = NormalizeNullable(req.Sku);
            var barcode = NormalizeNullable(req.Barcode);
            var serialNumber = NormalizeNullable(req.SerialNumber);

            var existing = await _db.Set<Tool>()
                .FirstOrDefaultAsync(x =>
                    (sku != null && x.Sku == sku) ||
                    (barcode != null && x.Barcode == barcode) ||
                    (serialNumber != null && x.SerialNumber == serialNumber), ct);

            if (existing != null)
            {
                if (existing.IsActive)
                    throw new InvalidOperationException("Tool with given SKU, Barcode or SerialNumber already exists.");

                existing.Sku = sku;
                existing.Name = req.Name.Trim();
                existing.Description = NormalizeNullable(req.Description);
                existing.CategoryId = req.CategoryId;
                existing.Manufacturer = NormalizeNullable(req.Manufacturer);
                existing.Model = NormalizeNullable(req.Model);
                existing.Barcode = barcode;
                existing.SerialNumber = serialNumber;
                existing.Url = req.Url.Trim();
                existing.Condition = req.Condition;
                existing.Status = req.Status;
                existing.PurchaseDate = req.PurchaseDate;
                existing.IsActive = true;
                existing.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.SaveChangesAsync(ct);

                if (req.StorageLocationId.HasValue)
                    await EnsureStorageStockAsync(existing.Id, req.StorageLocationId.Value, existing.UpdatedAt, ct);

                return existing.Id;
            }

            var now = DateTimeOffset.UtcNow;

            var entity = new Tool
            {
                Sku = sku,
                Name = req.Name.Trim(),
                Description = NormalizeNullable(req.Description),
                CategoryId = req.CategoryId,
                Manufacturer = NormalizeNullable(req.Manufacturer),
                Model = NormalizeNullable(req.Model),
                Barcode = barcode,
                SerialNumber = serialNumber,
                Url = req.Url.Trim(),
                Condition = req.Condition,
                Status = req.Status,
                PurchaseDate = req.PurchaseDate,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Add(entity);
            await _db.SaveChangesAsync(ct);

            if (req.StorageLocationId.HasValue)
                await EnsureStorageStockAsync(entity.Id, req.StorageLocationId.Value, now, ct);

            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long toolId, ToolUpdateRequest req, long managerUserId, CancellationToken ct)
        {
            var entity = await _db.Set<Tool>().FirstOrDefaultAsync(x => x.Id == toolId, ct);
            if (entity is null) return false;

            if (string.IsNullOrWhiteSpace(req.Name))
                throw new InvalidOperationException("Name is required.");

            if (string.IsNullOrWhiteSpace(req.Url))
                throw new InvalidOperationException("Url is required.");

            var sku = NormalizeNullable(req.Sku);
            var barcode = NormalizeNullable(req.Barcode);
            var serialNumber = NormalizeNullable(req.SerialNumber);

            var skuExists = sku != null && await _db.Set<Tool>()
                .AnyAsync(x => x.Id != toolId && x.Sku == sku, ct);

            if (skuExists)
                throw new InvalidOperationException("Tool with given SKU already exists.");

            var barcodeExists = barcode != null && await _db.Set<Tool>()
                .AnyAsync(x => x.Id != toolId && x.Barcode == barcode, ct);

            if (barcodeExists)
                throw new InvalidOperationException("Tool with given Barcode already exists.");

            var serialExists = serialNumber != null && await _db.Set<Tool>()
                .AnyAsync(x => x.Id != toolId && x.SerialNumber == serialNumber, ct);

            if (serialExists)
                throw new InvalidOperationException("Tool with given SerialNumber already exists.");

            entity.Sku = sku;
            entity.Name = req.Name.Trim();
            entity.Description = NormalizeNullable(req.Description);
            entity.CategoryId = req.CategoryId;
            entity.Manufacturer = NormalizeNullable(req.Manufacturer);
            entity.Model = NormalizeNullable(req.Model);
            entity.Barcode = barcode;
            entity.SerialNumber = serialNumber;
            entity.Url = req.Url.Trim();
            entity.Condition = req.Condition;
            entity.Status = req.Status;
            entity.PurchaseDate = req.PurchaseDate;
            entity.IsActive = req.IsActive;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> SoftDeleteAsync(long toolId, long managerUserId, CancellationToken ct)
        {
            var entity = await _db.Set<Tool>().FirstOrDefaultAsync(x => x.Id == toolId, ct);
            if (entity is null) return false;

            entity.IsActive = false;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<ToolOperationResultDto> AssignAsync(
            ToolAssignRequest req,
            long userId,
            bool isManager,
            CancellationToken ct)
        {
            if (req.ToolId <= 0)
                throw new InvalidOperationException("ToolId is required.");

            if (req.Destination is null || req.Destination.TargetId <= 0)
                throw new InvalidOperationException("Destination is required.");

            await EnsureTargetAccessAsync(userId, isManager, req.Destination.TargetType, req.Destination.TargetId, ct);

            var now = DateTimeOffset.UtcNow;

            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            var tool = await _db.Set<Tool>()
                .Include(x => x.ToolAssetStock)
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == req.ToolId, ct);

            if (tool is null)
                throw new InvalidOperationException("Tool not found.");

            if (!tool.IsActive)
                throw new InvalidOperationException("Tool is inactive.");

            var activeAssignment = tool.Assignments
                .Where(a => a.ReturnedAt == null)
                .OrderByDescending(a => a.AssignedAt)
                .FirstOrDefault();

            if (activeAssignment != null)
                activeAssignment.ReturnedAt = now;

            if (tool.ToolAssetStock != null)
                _db.Remove(tool.ToolAssetStock);

            var assignment = new ToolAssignment
            {
                ToolId = tool.Id,
                TargetType = req.Destination.TargetType,
                TargetId = req.Destination.TargetId,
                AssignedAt = now,
                ReturnedAt = null,
                Notes = req.Notes,
                CreatedByUserId = userId,
                CreatedAt = now
            };

            _db.Add(assignment);

            tool.Status = req.Destination.TargetType == ToolTransferTargetType.STORAGE
                ? ToolStatus.IN_STOCK
                : ToolStatus.ASSIGNED;

            tool.UpdatedAt = now;

            if (req.Destination.TargetType == ToolTransferTargetType.STORAGE)
            {
                _db.Add(new ToolStock
                {
                    ToolId = tool.Id,
                    StorageLocationId = req.Destination.TargetId,
                    UpdatedAt = now
                });
            }

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return new ToolOperationResultDto
            {
                ToolId = tool.Id
            };
        }

        public async Task<ToolOperationResultDto> ReturnToStorageAsync(
            long toolId,
            ToolReturnToStorageRequest req,
            long userId,
            bool isManager,
            CancellationToken ct)
        {
            if (toolId <= 0)
                throw new InvalidOperationException("ToolId is required.");

            if (req.StorageLocationId <= 0)
                throw new InvalidOperationException("StorageLocationId is required.");

            var now = DateTimeOffset.UtcNow;

            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            var tool = await _db.Set<Tool>()
                .Include(x => x.ToolAssetStock)
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == toolId, ct);

            if (tool is null)
                throw new InvalidOperationException("Tool not found.");

            var activeAssignment = tool.Assignments
                .Where(a => a.ReturnedAt == null)
                .OrderByDescending(a => a.AssignedAt)
                .FirstOrDefault();

            if (activeAssignment != null)
            {
                if (!isManager)
                    await EnsureTargetAccessAsync(userId, false, activeAssignment.TargetType, activeAssignment.TargetId, ct);

                activeAssignment.ReturnedAt = now;
                if (!string.IsNullOrWhiteSpace(req.Notes))
                    activeAssignment.Notes = req.Notes.Trim();
            }

            if (tool.ToolAssetStock == null)
            {
                _db.Add(new ToolStock
                {
                    ToolId = tool.Id,
                    StorageLocationId = req.StorageLocationId,
                    UpdatedAt = now
                });
            }
            else
            {
                tool.ToolAssetStock.StorageLocationId = req.StorageLocationId;
                tool.ToolAssetStock.UpdatedAt = now;
            }

            tool.Status = ToolStatus.IN_STOCK;
            tool.UpdatedAt = now;

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return new ToolOperationResultDto
            {
                ToolId = tool.Id
            };
        }

        public async Task<long> ReportIssueAsync(long toolId, ToolIssueCreateRequest req, long userId, CancellationToken ct)
        {
            if (toolId <= 0)
                throw new InvalidOperationException("ToolId is required.");

            if (string.IsNullOrWhiteSpace(req.Description))
                throw new InvalidOperationException("Description is required.");

            if (string.IsNullOrWhiteSpace(req.Url))
                throw new InvalidOperationException("Url is required.");

            var exists = await _db.Set<Tool>().AnyAsync(x => x.Id == toolId, ct);
            if (!exists)
                throw new InvalidOperationException("Tool not found.");

            var now = DateTimeOffset.UtcNow;

            var issue = new ToolIssue
            {
                ToolId = toolId,
                ReportedByUserId = userId,
                IssueType = req.IssueType,
                Description = req.Description.Trim(),
                Status = GenericStatus.NEW,
                Url = req.Url.Trim(),
                Caption = NormalizeNullable(req.Caption),
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Add(issue);
            await _db.SaveChangesAsync(ct);

            return issue.Id;
        }

        private async Task EnsureTargetAccessAsync(
            long userId,
            bool isManager,
            ToolTransferTargetType targetType,
            long targetId,
            CancellationToken ct)
        {
            if (isManager)
                return;

            switch (targetType)
            {
                case ToolTransferTargetType.STORAGE:
                    return;

                case ToolTransferTargetType.EMPLOYEE:
                    {
                        var employeeId = await GetEmployeeIdByUserIdAsync(userId, ct)
                            ?? throw new UnauthorizedAccessException("Employee profile not found.");

                        if (employeeId != targetId)
                            throw new UnauthorizedAccessException("No access to this employee target.");

                        return;
                    }

                default:
                    throw new InvalidOperationException("Unsupported target type.");
            }
        }

        private async Task<long?> GetEmployeeIdByUserIdAsync(long userId, CancellationToken ct)
        {
            return await _db.Set<Employee>()
                .AsNoTracking()
                .Where(e => e.Person!.UserId == userId)
                .Select(e => (long?)e.Id)
                .FirstOrDefaultAsync(ct);
        }

        private async Task EnsureStorageStockAsync(long toolId, long storageLocationId, DateTimeOffset now, CancellationToken ct)
        {
            var stock = await _db.Set<ToolStock>().FirstOrDefaultAsync(x => x.ToolId == toolId, ct);

            if (stock is null)
            {
                _db.Add(new ToolStock
                {
                    ToolId = toolId,
                    StorageLocationId = storageLocationId,
                    UpdatedAt = now
                });
            }
            else
            {
                stock.StorageLocationId = storageLocationId;
                stock.UpdatedAt = now;
            }

            var activeAssignments = await _db.Set<ToolAssignment>()
                .Where(x => x.ToolId == toolId && x.ReturnedAt == null)
                .ToListAsync(ct);

            foreach (var a in activeAssignments)
                a.ReturnedAt = now;

            var tool = await _db.Set<Tool>().FirstAsync(x => x.Id == toolId, ct);
            tool.Status = ToolStatus.IN_STOCK;
            tool.UpdatedAt = now;

            await _db.SaveChangesAsync(ct);
        }

        private static string? NormalizeNullable(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}