using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.WorkSessionModels;
using PowerPulseRestAPI.DTO.WorkSessionDto.Requests;
using PowerPulseRestAPI.DTO.WorkSessionDto.Responses;
using PowerPulseRestAPI.Mappers.WorkSessions;
using PowerPulseRestAPI.Services.WorkSessionS;
using System.Security.Claims;

namespace PowerPulseRestAPI.Services
{
    public class WorkSessionService : IWorkSessionService
    {
        private readonly PowerPulseContext _dbContext;

        public WorkSessionService(PowerPulseContext db)
        {
            _dbContext = db;
        }

        public async Task<WorkSessionListItemDto> CreateAsync(
            ClaimsPrincipal user,
            CreateWorkSessionDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            EnsureOwnershipOrAdmin(user, dto.EmployeeId);

            await EnsureEmployeeExistsAsync(dto.EmployeeId, cancellationToken);
            await EnsureProjectExistsAsync(dto.ProjectId, cancellationToken);
            ValidateDateRange(dto.StartedAt, dto.EndedAt);

            await EnsureNoOtherActiveSessionAsync(
                dto.EmployeeId,
                excludedSessionId: null,
                cancellationToken);

            var now = DateTimeOffset.UtcNow;
            var status = GetStatus(dto.EndedAt);

            var workSession = new WorkSession
            {
                EmployeeId = dto.EmployeeId,
                ProjectId = dto.ProjectId,
                StartedAt = dto.StartedAt,
                EndedAt = dto.EndedAt,
                Status = status,
                Note = dto.Note,
                CreatedAt = now,
                UpdatedAt = now
            };

            _dbContext.WorkSessions.Add(workSession);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var created = await GetQuery()
                .FirstAsync(x => x.Id == workSession.Id, cancellationToken);

            return created.ToListItemDto();
        }

        public async Task<IReadOnlyList<WorkSessionListItemDto>> GetListAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken = default)
        {
            var query = GetQuery();

            if (!IsAdmin(user))
            {
                var currentEmployeeId = GetCurrentEmployeeId(user);
                query = query.Where(x => x.EmployeeId == currentEmployeeId);
            }

            var sessions = await query
                .OrderByDescending(x => x.StartedAt)
                .ToListAsync(cancellationToken);

            return sessions
                .Select(x => x.ToListItemDto())
                .ToList();
        }

        public async Task<WorkSessionListItemDto> GetByIdAsync(
            ClaimsPrincipal user,
            long id,
            CancellationToken cancellationToken = default)
        {
            var session = await GetAccessibleSessionDetailsAsync(user, id, cancellationToken);
            return session.ToListItemDto();
        }

        public async Task<IReadOnlyList<EmployeeWorkSessionsListItemDto>> GetByEmployeeIdAsync(
            ClaimsPrincipal user,
            long employeeId,
            CancellationToken cancellationToken = default)
        {
            EnsureOwnershipOrAdmin(user, employeeId);
            await EnsureEmployeeExistsAsync(employeeId, cancellationToken);

            var sessions = await GetQuery()
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.StartedAt)
                .ToListAsync(cancellationToken);

            return sessions
                .Select(x => x.ToEmployeeListItemDto())
                .ToList();
        }

        public async Task<WorkSessionListItemDto> UpdateAsync(
            ClaimsPrincipal user,
            long id,
            UpdateWorkSessionDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var session = await GetOwnedOrAccessibleSessionAsync(user, id, cancellationToken);

            if (!IsAdmin(user) && dto.EmployeeId != session.EmployeeId)
            {
                throw new UnauthorizedAccessException("You cannot reassign this work session to another employee.");
            }

            await EnsureEmployeeExistsAsync(dto.EmployeeId, cancellationToken);
            await EnsureProjectExistsAsync(dto.ProjectId, cancellationToken);
            ValidateDateRange(dto.StartedAt, dto.EndedAt);

            var status = GetStatus(dto.EndedAt);

            if (status == WorkSessionStatus.ACTIVE)
            {
                await EnsureNoOtherActiveSessionAsync(
                    dto.EmployeeId,
                    excludedSessionId: id,
                    cancellationToken);
            }

            session.EmployeeId = dto.EmployeeId;
            session.ProjectId = dto.ProjectId;
            session.StartedAt = dto.StartedAt;
            session.EndedAt = dto.EndedAt;
            session.Status = status;
            session.Note = dto.Note;
            session.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var updated = await GetQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToListItemDto();
        }

        public async Task<WorkSessionListItemDto> CloseAsync(
            ClaimsPrincipal user,
            long id,
            CloseWorkSessionDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var session = await GetOwnedOrAccessibleSessionAsync(user, id, cancellationToken);

            if (session.Status != WorkSessionStatus.ACTIVE || session.EndedAt.HasValue)
            {
                throw new InvalidOperationException("Only an active work session can be closed.");
            }

            if (dto.EndedAt < session.StartedAt)
            {
                throw new InvalidOperationException("EndedAt cannot be earlier than StartedAt.");
            }

            session.EndedAt = dto.EndedAt;
            session.Note = dto.Note ?? session.Note;
            session.Status = WorkSessionStatus.CLOSED;
            session.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var updated = await GetQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToListItemDto();
        }

        public async Task DeleteAsync(
            ClaimsPrincipal user,
            long id,
            CancellationToken cancellationToken = default)
        {
            var session = await GetOwnedOrAccessibleSessionAsync(user, id, cancellationToken);

            if (session.InvoiceId.HasValue)
            {
                throw new InvalidOperationException("An invoiced work session cannot be deleted.");
            }

            _dbContext.WorkSessions.Remove(session);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<WorkSession> GetQuery()
        {
            return _dbContext.WorkSessions
                .AsNoTracking()
                .Include(x => x.Employee)
                    .ThenInclude(x => x.Person)
                .Include(x => x.Project);
        }

        private async Task<WorkSession> GetOwnedOrAccessibleSessionAsync(
            ClaimsPrincipal user,
            long id,
            CancellationToken cancellationToken)
        {
            var session = await _dbContext.WorkSessions
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (session is null)
            {
                throw new KeyNotFoundException($"Work session with id '{id}' was not found.");
            }

            EnsureOwnershipOrAdmin(user, session.EmployeeId);

            return session;
        }

        private async Task<WorkSession> GetAccessibleSessionDetailsAsync(
            ClaimsPrincipal user,
            long id,
            CancellationToken cancellationToken)
        {
            var session = await GetQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (session is null)
            {
                throw new KeyNotFoundException($"Work session with id '{id}' was not found.");
            }

            EnsureOwnershipOrAdmin(user, session.EmployeeId);

            return session;
        }

        private async Task EnsureEmployeeExistsAsync(
            long employeeId,
            CancellationToken cancellationToken)
        {
            var employeeExists = await _dbContext.Employees
                .AnyAsync(x => x.Id == employeeId, cancellationToken);

            if (!employeeExists)
            {
                throw new KeyNotFoundException($"Employee with id '{employeeId}' was not found.");
            }
        }

        private async Task EnsureProjectExistsAsync(
            long projectId,
            CancellationToken cancellationToken)
        {
            var projectExists = await _dbContext.Projects
                .AnyAsync(x => x.Id == projectId, cancellationToken);

            if (!projectExists)
            {
                throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
            }
        }

        private async Task EnsureNoOtherActiveSessionAsync(
            long employeeId,
            long? excludedSessionId,
            CancellationToken cancellationToken)
        {
            var hasOpenSession = await _dbContext.WorkSessions.AnyAsync(
                x => x.EmployeeId == employeeId &&
                     x.Id != (excludedSessionId ?? 0) &&
                     x.EndedAt == null &&
                     x.Status == WorkSessionStatus.ACTIVE,
                cancellationToken);

            if (hasOpenSession)
            {
                throw new InvalidOperationException("Employee already has another active work session.");
            }
        }

        private static WorkSessionStatus GetStatus(DateTimeOffset? endedAt)
        {
            return endedAt.HasValue
                ? WorkSessionStatus.CLOSED
                : WorkSessionStatus.ACTIVE;
        }

        private static void ValidateDateRange(DateTimeOffset startedAt, DateTimeOffset? endedAt)
        {
            if (endedAt.HasValue && endedAt.Value < startedAt)
            {
                throw new InvalidOperationException("EndedAt cannot be earlier than StartedAt.");
            }
        }

        private static void EnsureOwnershipOrAdmin(
            ClaimsPrincipal user,
            long employeeId)
        {
            if (IsAdmin(user))
            {
                return;
            }

            var currentEmployeeId = GetCurrentEmployeeId(user);

            if (employeeId != currentEmployeeId)
            {
                throw new UnauthorizedAccessException("Access denied.");
            }
        }

        private static bool IsAdmin(ClaimsPrincipal user)
        {
            return user.IsInRole("ADMIN");
        }

        private static long GetCurrentEmployeeId(ClaimsPrincipal user)
        {
            var employeeIdClaim = user.FindFirst("employeeId")?.Value;

            if (string.IsNullOrWhiteSpace(employeeIdClaim))
            {
                throw new UnauthorizedAccessException("Missing 'employeeId' claim in the token.");
            }

            if (!long.TryParse(employeeIdClaim, out var employeeId))
            {
                throw new UnauthorizedAccessException("Invalid 'employeeId' claim.");
            }

            return employeeId;
        }
    }
}