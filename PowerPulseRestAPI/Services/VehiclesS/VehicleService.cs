using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.VehicleModels;
using PowerPulseRestAPI.DTO.VehicleDto.Requests;
using PowerPulseRestAPI.DTO.VehicleDto.Responses;
using PowerPulseRestAPI.Mappers.Vehicles;
using PowerPulseRestAPI.Services.Uploads;

namespace PowerPulseRestAPI.Services.VehiclesS
{
    public class VehicleService : IVehicleService
    {
        private readonly PowerPulseContext _dbContext;
        private readonly IFileStorageService _fileStorageService;

        public VehicleService(PowerPulseContext db,
            IFileStorageService fileStorageService)
        {
            _dbContext = db;
            _fileStorageService = fileStorageService;
        }

        public async Task<VehicleDetailDto> CreateAsync(
            CreateVehicleDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var plateExists = await _dbContext.Vehicles
                .IgnoreQueryFilters()
                .AnyAsync(x => x.PlateNumber == dto.PlateNumber, cancellationToken);

            if (plateExists)
            {
                throw new InvalidOperationException($"Vehicle with plate number '{dto.PlateNumber}' already exists.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Vin))
            {
                var vinExists = await _dbContext.Vehicles
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Vin == dto.Vin, cancellationToken);

                if (vinExists)
                {
                    throw new InvalidOperationException($"Vehicle with VIN '{dto.Vin}' already exists.");
                }
            }

            var now = DateTimeOffset.UtcNow;

            var vehicle = new Vehicle
            {
                PlateNumber = dto.PlateNumber,
                Vin = dto.Vin,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                Url = dto.Url,
                Caption = dto.Caption,
                Status = dto.Status,
                CurrentMileage = dto.CurrentMileage,
                LastServiceAt = dto.LastServiceAt,
                LastServiceMileage = dto.LastServiceMileage,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false,
            };

            _dbContext.Vehicles.Add(vehicle);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var created = await GetQuery()
                .FirstAsync(x => x.Id == vehicle.Id, cancellationToken);

            return created.ToDetailDto();
        }

        public async Task<IReadOnlyList<VehicleListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default)
        {
            var vehicles = await GetQuery()
                .OrderBy(x => x.PlateNumber)
                .ToListAsync(cancellationToken);

            return vehicles
                .Select(x => x.ToListItemDto())
                .ToList();
        }

        public async Task<VehicleDetailDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var vehicle = await GetQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (vehicle is null)
            {
                throw new KeyNotFoundException($"Vehicle with id '{id}' was not found.");
            }

            return vehicle.ToDetailDto();
        }

        public async Task<VehicleListItemDto> GetByEmployeeAsync(
            long employeeId,
            CancellationToken cancellationToken = default)
        {
            var vehicle = await GetQuery()
                .Where(x => x.Assignments.Any(a => a.EmployeeId == employeeId && a.ReturnedAt == null))
                .FirstOrDefaultAsync(cancellationToken);

            if (vehicle is null)
            {
                throw new KeyNotFoundException($"No active vehicle assignment found for employee with id '{employeeId}'.");
            }

            return vehicle.ToListItemDto();
        }

        public async Task<VehicleDetailDto> UpdateAsync(
            long id,
            UpdateVehicleDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var vehicle = await _dbContext.Vehicles
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (vehicle is null)
            {
                throw new KeyNotFoundException($"Vehicle with id '{id}' was not found.");
            }

            var plateExists = await _dbContext.Vehicles
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Id != id && x.PlateNumber == dto.PlateNumber, cancellationToken);

            if (plateExists)
            {
                throw new InvalidOperationException($"Vehicle with plate number '{dto.PlateNumber}' already exists.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Vin))
            {
                var vinExists = await _dbContext.Vehicles
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Id != id && x.Vin == dto.Vin, cancellationToken);

                if (vinExists)
                {
                    throw new InvalidOperationException($"Vehicle with VIN '{dto.Vin}' already exists.");
                }
            }

            var oldVehicleUrl = vehicle.Url;

            vehicle.PlateNumber = dto.PlateNumber;
            vehicle.Vin = dto.Vin;
            vehicle.Make = dto.Make;
            vehicle.Model = dto.Model;
            vehicle.Year = dto.Year;
            vehicle.Url = dto.Url;
            vehicle.Caption = dto.Caption;
            vehicle.Status = dto.Status;
            vehicle.CurrentMileage = dto.CurrentMileage;
            vehicle.LastServiceAt = dto.LastServiceAt;
            vehicle.LastServiceMileage = dto.LastServiceMileage;
            vehicle.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            if (!string.Equals(oldVehicleUrl, vehicle.Url, StringComparison.OrdinalIgnoreCase))
            {
                _fileStorageService.DeleteFileByUrl(oldVehicleUrl);
            }

            var updated = await GetQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailDto();
        }

        public async Task<VehicleDetailDto> AssignAsync(
            long id,
            CreateVehicleAssignmentDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var vehicle = await _dbContext.Vehicles
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (vehicle is null)
            {
                throw new KeyNotFoundException($"Vehicle with id '{id}' was not found.");
            }

            var employeeExists = await _dbContext.Employees
                .AnyAsync(x => x.Id == dto.EmployeeId, cancellationToken);

            if (!employeeExists)
            {
                throw new KeyNotFoundException($"Employee with id '{dto.EmployeeId}' was not found.");
            }

            var activeVehicleAssignment = vehicle.Assignments.Any(x => x.ReturnedAt == null);
            if (activeVehicleAssignment)
            {
                throw new InvalidOperationException("Vehicle is already assigned.");
            }

            var employeeHasActiveVehicle = await _dbContext.VehicleAssignments
                .AnyAsync(x => x.EmployeeId == dto.EmployeeId && x.ReturnedAt == null, cancellationToken);

            if (employeeHasActiveVehicle)
            {
                throw new InvalidOperationException("Employee already has an active vehicle assignment.");
            }

            var now = DateTimeOffset.UtcNow;

            var assignment = new VehicleAssignment
            {
                VehicleId = vehicle.Id,
                EmployeeId = dto.EmployeeId,
                AssignedAt = now,
                ReturnedAt = null,
                Note = dto.Note,
                CreatedAt = now
            };

            _dbContext.VehicleAssignments.Add(assignment);

            vehicle.Status = VehicleStatus.ASSIGNED;
            vehicle.UpdatedAt = now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var updated = await GetQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailDto();
        }

        public async Task<VehicleDetailDto> ReturnAsync(
            long id,
            string? note,
            CancellationToken cancellationToken = default)
        {
            var vehicle = await _dbContext.Vehicles
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (vehicle is null)
            {
                throw new KeyNotFoundException($"Vehicle with id '{id}' was not found.");
            }

            var assignment = vehicle.Assignments
                .OrderByDescending(x => x.AssignedAt)
                .FirstOrDefault(x => x.ReturnedAt == null);

            if (assignment is null)
            {
                throw new InvalidOperationException("Vehicle is not currently assigned.");
            }

            var now = DateTimeOffset.UtcNow;

            assignment.ReturnedAt = now;
            assignment.Note = string.IsNullOrWhiteSpace(note)
                ? assignment.Note
                : note;

            vehicle.Status = VehicleStatus.AVAILABLE;
            vehicle.UpdatedAt = now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var updated = await GetQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailDto();
        }

        public async Task<VehicleIssueListItemDto> CreateIssueAsync(
            long vehicleId,
            CreateVehicleIssueDto dto,
            long reportedByUserId,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var vehicleExists = await _dbContext.Vehicles
                .AnyAsync(x => x.Id == vehicleId, cancellationToken);

            if (!vehicleExists)
            {
                throw new KeyNotFoundException($"Vehicle with id '{vehicleId}' was not found.");
            }

            var userExists = await _dbContext.Users
                .AnyAsync(x => x.Id == reportedByUserId, cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException($"User with id '{reportedByUserId}' was not found.");
            }

            var now = DateTimeOffset.UtcNow;

            var issue = new VehicleIssue
            {
                VehicleId = vehicleId,
                ReportedByUserId = reportedByUserId,
                Description = dto.Description,
                Status = dto.Status,
                CreatedAt = now,
                UpdatedAt = now
            };

            _dbContext.VehicleIssues.Add(issue);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var created = await _dbContext.VehicleIssues
                .AsNoTracking()
                .Include(x => x.ReportedByUser)
                    .ThenInclude(x => x.Person)
                .FirstAsync(x => x.Id == issue.Id, cancellationToken);

            return created.ToIssueDto();
        }

        public async Task<VehicleIssueListItemDto> UpdateIssueAsync(
            long vehicleId,
            long issueId,
            UpdateVehicleIssueDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var issue = await _dbContext.VehicleIssues
                .Include(x => x.ReportedByUser)
                    .ThenInclude(x => x.Person)
                .FirstOrDefaultAsync(x => x.Id == issueId && x.VehicleId == vehicleId, cancellationToken);

            if (issue is null)
            {
                throw new KeyNotFoundException($"Vehicle issue with id '{issueId}' was not found.");
            }

            issue.Status = dto.Status;
            issue.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return issue.ToIssueDto();
        }

        public async Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var vehicle = await _dbContext.Vehicles
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (vehicle is null)
            {
                throw new KeyNotFoundException($"Vehicle with id '{id}' was not found.");
            }

            var activeAssignmentExists = vehicle.Assignments.Any(x => x.ReturnedAt == null);
            if (activeAssignmentExists)
            {
                throw new InvalidOperationException("Assigned vehicle cannot be deleted.");
            }

            vehicle.IsDeleted = true;
            vehicle.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }


        public async Task DeleteIssueAsync(
            long vehicleId,
            long issueId,
            CancellationToken cancellationToken = default)
        {
            var issue = await _dbContext.VehicleIssues
                .FirstOrDefaultAsync(x => x.Id == issueId && x.VehicleId == vehicleId, cancellationToken);

            if (issue is null)
            {
                throw new KeyNotFoundException($"Vehicle issue with id '{issueId}' was not found.");
            }

            _dbContext.VehicleIssues.Remove(issue);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<Vehicle> GetQuery()
        {
            return _dbContext.Vehicles
                .AsNoTracking()
                .Include(x => x.Assignments)
                    .ThenInclude(x => x.Employee)
                        .ThenInclude(x => x.Person)
                .Include(x => x.Issues)
                    .ThenInclude(x => x.ReportedByUser)
                        .ThenInclude(x => x.Person);
        }
    }
}