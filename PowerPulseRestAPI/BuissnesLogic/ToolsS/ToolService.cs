using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.DTO.ToolDto.Requests;
using PowerPulseRestAPI.DTO.ToolDto.Responses;
using PowerPulseRestAPI.Mappers.Tools;
using PowerPulseRestAPI.BuissnesLogic.ToolsS;
using PowerPulseRestAPI.BuissnesLogic.Uploads;
using System;

namespace PowerPulseRestAPI.BuissnesLogic
{
    public class ToolService : IToolService
    {
        private readonly PowerPulseContext _dbContext;
        private readonly IFileStorageService _fileStorageService;

        public ToolService(PowerPulseContext db, IFileStorageService fileStorageService)
        {
            _dbContext = db;
            _fileStorageService = fileStorageService;
        }

        public async Task<ToolDetailsDto> CreateAsync(
            CreateToolDto dto,
            long createdByUserId,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var categoryExists = await _dbContext.ToolCategories
                .AnyAsync(x => x.Id == dto.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new KeyNotFoundException($"Tool category with id '{dto.CategoryId}' was not found.");
            }

            var userExists = await _dbContext.Users
                .AnyAsync(x => x.Id == createdByUserId, cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException($"User with id '{createdByUserId}' was not found.");
            }

            if (!string.IsNullOrWhiteSpace(dto.SerialNumber))
            {
                var serialExists = await _dbContext.Tools
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.SerialNumber == dto.SerialNumber, cancellationToken);

                if (serialExists)
                {
                    throw new InvalidOperationException($"Tool with serial number '{dto.SerialNumber}' already exists.");
                }
            }

            var now = DateTimeOffset.UtcNow;

            var tool = new Tool
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Manufacturer = dto.Manufacturer,
                Model = dto.Model,
                SerialNumber = dto.SerialNumber,
                Url = dto.Url,
                Condition = dto.Condition,
                Status = ToolStatus.IN_STOCK,
                PurchaseDate = dto.PurchaseDate,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false,
            };

            _dbContext.Tools.Add(tool);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var created = await GetQuery()
                .FirstAsync(x => x.Id == tool.Id, cancellationToken);

            return created.ToDetailsDto();
        }

        public async Task<IReadOnlyList<ToolListItemDto>> GetListAsync(
            ToolFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            filter ??= new ToolFilterDto();

            var query = GetQuery();

            query = ApplyFilters(query, filter);
            query = ApplySorting(query, filter);

            var tools = await query
                .ToListAsync(cancellationToken);

            return tools
                .Select(x => x.ToListItemDto())
                .ToList();
        }
        
        public async Task<ToolStatsDto> GetStatsAsync(
            ToolFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            filter ??= new ToolFilterDto();

            var query = _dbContext.Tools
                .AsNoTracking()
                .AsQueryable();

            query = ApplyFilters(query, filter);

            var summary = await query
                .GroupBy(_ => 1)
                .Select(group => new ToolStatsSummaryDto
                {
                    TotalTools = group.Count(),

                    InStockTools = group.Count(x =>
                        x.Status == ToolStatus.IN_STOCK),

                    AssignedTools = group.Count(x =>
                        x.Status == ToolStatus.ASSIGNED),

                    InServiceTools = group.Count(x =>
                        x.Status == ToolStatus.IN_SERVICE)
                })
                .FirstOrDefaultAsync(cancellationToken);

            return new ToolStatsDto
            {
                Summary = summary ?? new ToolStatsSummaryDto()
            };
        }
        
        public async Task<IReadOnlyList<RecentToolAssignmentDto>>
            GetRecentAssignmentsAsync(
                int limit = 3,
                CancellationToken cancellationToken = default)
        {
            var safeLimit = Math.Clamp(limit, 1, 20);

            return await _dbContext.ToolAssignments
                .AsNoTracking()
                .Where(x => !x.Tool.IsDeleted)
                .OrderByDescending(x => x.AssignedAt)
                .ThenByDescending(x => x.Id)
                .Take(safeLimit)
                .Select(x => new RecentToolAssignmentDto
                {
                    ToolId = x.ToolId,
                    ToolName = x.Tool.Name,
                    Manufacturer = x.Tool.Manufacturer,
                    Model = x.Tool.Model,

                    Assignment = new ToolAssignmentDto
                    {
                        Id = x.Id,
                        EmployeeId = x.EmployeeId,

                        EmployeeName =
                            x.Employee.Person.FirstName + " " +
                            x.Employee.Person.LastName,

                        AssignedAt = x.AssignedAt,
                        ReturnedAt = x.ReturnedAt,
                        Notes = x.Notes
                    }
                })
                .ToListAsync(cancellationToken);
        }
        
        public async Task<IReadOnlyList<ToolEmployeeSelectOptionDto>>
            GetEmployeeSelectOptionsAsync(
                CancellationToken cancellationToken = default)
        {
            return await _dbContext.Employees
                .AsNoTracking()
                .OrderBy(x => x.Person.LastName)
                .ThenBy(x => x.Person.FirstName)
                .Select(x => new ToolEmployeeSelectOptionDto
                {
                    Value = x.Id,

                    Label =
                        x.Person.FirstName + " " +
                        x.Person.LastName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<ToolDetailsDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var tool = await GetQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (tool is null)
            {
                throw new KeyNotFoundException($"Tool with id '{id}' was not found.");
            }

            return tool.ToDetailsDto();
        }

        public async Task<ToolDetailsDto> UpdateAsync(
            long id,
            UpdateToolDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var tool = await _dbContext.Tools
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (tool is null)
            {
                throw new KeyNotFoundException($"Tool with id '{id}' was not found.");
            }

            var categoryExists = await _dbContext.ToolCategories
                .AnyAsync(x => x.Id == dto.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new KeyNotFoundException($"Tool category with id '{dto.CategoryId}' was not found.");
            }

            if (!string.IsNullOrWhiteSpace(dto.SerialNumber))
            {
                var serialExists = await _dbContext.Tools
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Id != id && x.SerialNumber == dto.SerialNumber, cancellationToken);

                if (serialExists)
                {
                    throw new InvalidOperationException($"Tool with serial number '{dto.SerialNumber}' already exists.");
                }
            }

            var oldToolUrl = tool.Url;

            tool.Name = dto.Name;
            tool.Description = dto.Description;
            tool.CategoryId = dto.CategoryId;
            tool.Manufacturer = dto.Manufacturer;
            tool.Model = dto.Model;
            tool.SerialNumber = dto.SerialNumber;
            tool.Url = dto.Url;
            tool.Condition = dto.Condition;
            tool.PurchaseDate = dto.PurchaseDate;
            tool.IsActive = dto.IsActive;
            tool.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            if (!string.Equals(oldToolUrl, tool.Url, StringComparison.OrdinalIgnoreCase))
            {
                _fileStorageService.DeleteFileByUrl(oldToolUrl);
            }

            var updated = await GetQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailsDto();
        }

        public async Task<ToolDetailsDto> AssignAsync(
            string role,
            long employeeId,
            long id,
            AssignToolDto dto,
            long createdByUserId,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var isAdmin = string.Equals(role, "ADMIN", StringComparison.OrdinalIgnoreCase);

            if (!isAdmin && dto.EmployeeId != employeeId)
            {
                throw new UnauthorizedAccessException(
                    "User can assign tool only to himself.");
            }

            var tool = await _dbContext.Tools
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (tool is null)
            {
                throw new KeyNotFoundException($"Tool with id '{id}' was not found.");
            }

            var employeeExists = await _dbContext.Employees
                .AnyAsync(x => x.Id == dto.EmployeeId, cancellationToken);

            if (!employeeExists)
            {
                throw new KeyNotFoundException($"Employee with id '{dto.EmployeeId}' was not found.");
            }

            var userExists = await _dbContext.Users
                .AnyAsync(x => x.Id == createdByUserId, cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException($"User with id '{createdByUserId}' was not found.");
            }

            var activeAssignmentExists = tool.Assignments.Any(x => x.ReturnedAt == null);

            if (activeAssignmentExists)
            {
                throw new InvalidOperationException("Tool is already assigned.");
            }

            var now = DateTimeOffset.UtcNow;

            var assignment = new ToolAssignment
            {
                ToolId = tool.Id,
                EmployeeId = dto.EmployeeId,
                AssignedAt = now,
                ReturnedAt = null,
                Notes = dto.Notes,
                CreatedByUserId = createdByUserId,
                CreatedAt = now
            };

            _dbContext.ToolAssignments.Add(assignment);

            tool.Status = ToolStatus.ASSIGNED;
            tool.UpdatedAt = now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var updated = await GetQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailsDto();
        }

        public async Task<ToolDetailsDto> ReturnAsync(
            string role,
            long employeeId,
            long id,
            ReturnToolDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var isAdmin = string.Equals(role, "ADMIN", StringComparison.OrdinalIgnoreCase);

            var tool = await _dbContext.Tools
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (tool is null)
            {
                throw new KeyNotFoundException($"Tool with id '{id}' was not found.");
            }

            var assignment = tool.Assignments
                .OrderByDescending(x => x.AssignedAt)
                .FirstOrDefault(x => x.ReturnedAt == null);

            if (assignment is null)
            {
                throw new InvalidOperationException("Tool is not currently assigned.");
            }

            if (!isAdmin && assignment.EmployeeId != employeeId)
            {
                throw new UnauthorizedAccessException(
                    "User can return only tools assigned to himself.");
            }

            var now = DateTimeOffset.UtcNow;

            assignment.ReturnedAt = now;
            assignment.Notes = string.IsNullOrWhiteSpace(dto.Notes)
                ? assignment.Notes
                : dto.Notes;

            tool.Status = ToolStatus.IN_STOCK;
            tool.UpdatedAt = now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var updated = await GetQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailsDto();
        }

        public async Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var tool = await _dbContext.Tools
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (tool is null)
            {
                throw new KeyNotFoundException($"Tool with id '{id}' was not found.");
            }

            var activeAssignmentExists = tool.Assignments.Any(x => x.ReturnedAt == null);
            if (activeAssignmentExists)
            {
                throw new InvalidOperationException("Assigned tool cannot be deleted.");
            }

            tool.IsDeleted = true;
            tool.UpdatedAt = DateTimeOffset.UtcNow;
            tool.IsActive = false;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<Tool> GetQuery()
        {
            return _dbContext.Tools
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Assignments)
                    .ThenInclude(x => x.Employee)
                        .ThenInclude(x => x.Person);
        }
        
        private static IQueryable<Tool> ApplyFilters(
            IQueryable<Tool> query,
            ToolFilterDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim();
                var likeSearch = $"%{search}%";
                var isIdSearch = long.TryParse(search, out var toolId);

                query = query.Where(x =>
                    (isIdSearch && x.Id == toolId) ||

                    EF.Functions.Like(x.Name, likeSearch) ||

                    (
                        x.Manufacturer != null &&
                        EF.Functions.Like(x.Manufacturer, likeSearch)
                    ) ||

                    (
                        x.Model != null &&
                        EF.Functions.Like(x.Model, likeSearch)
                    ) ||

                    (
                        x.SerialNumber != null &&
                        EF.Functions.Like(x.SerialNumber, likeSearch)
                    ) ||

                    (
                        x.Category != null &&
                        EF.Functions.Like(x.Category.Name, likeSearch)
                    ));
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == filter.Status.Value);
            }

            if (filter.EmployeeId.HasValue)
            {
                query = query.Where(x =>
                    x.Assignments.Any(assignment =>
                        assignment.EmployeeId == filter.EmployeeId.Value &&
                        assignment.ReturnedAt == null));
            }

            return query;
        }
        
        private static IQueryable<Tool> ApplySorting(
            IQueryable<Tool> query,
            ToolFilterDto filter)
        {
            var sortBy = filter.SortBy?
                .Trim()
                .ToLowerInvariant();

            var descending = string.Equals(
                filter.SortOrder,
                "desc",
                StringComparison.OrdinalIgnoreCase);

            return sortBy switch
            {
                "id" or "toolid" => descending
                    ? query
                        .OrderByDescending(x => x.Id)
                        .ThenBy(x => x.Name)
                    : query
                        .OrderBy(x => x.Id)
                        .ThenBy(x => x.Name),

                "name" or "toolname" => descending
                    ? query
                        .OrderByDescending(x => x.Name)
                        .ThenBy(x => x.Id)
                    : query
                        .OrderBy(x => x.Name)
                        .ThenBy(x => x.Id),

                "category" or "categoryname" => descending
                    ? query
                        .OrderByDescending(x => x.Category.Name)
                        .ThenBy(x => x.Name)
                    : query
                        .OrderBy(x => x.Category.Name)
                        .ThenBy(x => x.Name),

                "manufacturer" => descending
                    ? query
                        .OrderByDescending(x => x.Manufacturer)
                        .ThenBy(x => x.Name)
                    : query
                        .OrderBy(x => x.Manufacturer)
                        .ThenBy(x => x.Name),

                "model" => descending
                    ? query
                        .OrderByDescending(x => x.Model)
                        .ThenBy(x => x.Name)
                    : query
                        .OrderBy(x => x.Model)
                        .ThenBy(x => x.Name),

                "status" => descending
                    ? query
                        .OrderByDescending(x => x.Status)
                        .ThenBy(x => x.Name)
                    : query
                        .OrderBy(x => x.Status)
                        .ThenBy(x => x.Name),

                _ => query
                    .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
            };
        }
    }
}