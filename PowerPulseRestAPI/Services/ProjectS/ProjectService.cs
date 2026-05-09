using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Models.AddressModels;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.DTO.ProjectDto.Requests;
using PowerPulseRestAPI.DTO.ProjectDto.Responses;
using PowerPulseRestAPI.Mappers.Projects;
using PowerPulseRestAPI.Services.ProjectS;
using PowerPulseRestAPI.Services.Uploads;

namespace PowerPulseRestAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly PowerPulseContext _dbContext;
        private readonly IFileStorageService _fileStorageService;

        public ProjectService(PowerPulseContext db, IFileStorageService fileStorageService)
        {
            _dbContext = db;
            _fileStorageService = fileStorageService;
        }

        public async Task<ProjectShortDetailsDto> CreateAsync(
            CreateProjectDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var codeExists = await _dbContext.Projects
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Code == dto.Code, cancellationToken);

            if (codeExists)
            {
                throw new InvalidOperationException($"Project with code '{dto.Code}' already exists.");
            }

            var nameExists = await _dbContext.Projects
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Name == dto.Name, cancellationToken);

            if (nameExists)
            {
                throw new InvalidOperationException($"Project with name '{dto.Name}' already exists.");
            }

            var customerExists = await _dbContext.Customers
                .AnyAsync(x => x.Id == dto.CustomerId, cancellationToken);

            if (!customerExists)
            {
                throw new KeyNotFoundException($"Customer with id '{dto.CustomerId}' was not found.");
            }

            var createdByEmployeeExists = await _dbContext.Employees
                .AnyAsync(x => x.Id == dto.CreatedByEmployeeId, cancellationToken);

            if (!createdByEmployeeExists)
            {
                throw new KeyNotFoundException($"Employee with id '{dto.CreatedByEmployeeId}' was not found.");
            }

            if (dto.ResponsibleEmployeeId.HasValue)
            {
                var employeeExists = await _dbContext.Employees
                    .AnyAsync(x => x.Id == dto.ResponsibleEmployeeId.Value, cancellationToken);

                if (!employeeExists)
                {
                    throw new KeyNotFoundException($"Employee with id '{dto.ResponsibleEmployeeId.Value}' was not found.");
                }
            }

            if (dto.EndDate.HasValue && dto.EndDate < dto.StartDate)
            {
                throw new InvalidOperationException("EndDate cannot be earlier than StartDate.");
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            var now = DateTimeOffset.UtcNow;

            var address = new Address
            {
                Country = dto.Address.Country,
                PostalCode = dto.Address.PostalCode,
                City = dto.Address.City,
                Street = dto.Address.Street,
                BuildingNumber = dto.Address.BuildingNumber,
                ApartmentNumber = dto.Address.ApartmentNumber,
                FullText = dto.Address.FullText,
                AddressType = dto.Address.AddressType,
                CreatedAt = now,
                UpdatedAt = now
            };

            _dbContext.Addresses.Add(address);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var project = new Project
            {
                CustomerId = dto.CustomerId,
                Code = dto.Code,
                Name = dto.Name,
                AvatarUrl = dto.AvatarUrl,
                Description = dto.Description,
                Status = dto.Status,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedByEmployeeId = dto.CreatedByEmployeeId,
                ResponsibleEmployeeId = dto.ResponsibleEmployeeId,
                AddressId = address.Id,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false,
            };

            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            var created = await GetProjectQuery()
                .FirstAsync(x => x.Id == project.Id, cancellationToken);

            return created.ToShortDetailsDto();
        }

        public async Task<IReadOnlyList<ProjectListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default)
        {
            var projects = await GetProjectQuery()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            return projects
                .Select(x => x.ToListItemDto())
                .ToList();
        }

        public async Task<ProjectFullDetailsDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var project = await GetProjectQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (project is null)
            {
                throw new KeyNotFoundException($"Project with id '{id}' was not found.");
            }

            return project.ToFullDetailsDto();
        }

        public async Task<ProjectShortDetailsDto> UpdateAsync(
            long id,
            UpdateProjectDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var project = await _dbContext.Projects
                .Include(x => x.Address)
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (project is null)
            {
                throw new KeyNotFoundException($"Project with id '{id}' was not found.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                var codeExists = await _dbContext.Projects
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Id != id && x.Code == dto.Code, cancellationToken);

                if (codeExists)
                {
                    throw new InvalidOperationException($"Project with code '{dto.Code}' already exists.");
                }

                project.Code = dto.Code;
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                var nameExists = await _dbContext.Projects
                    .IgnoreQueryFilters()
                    .AnyAsync(x => x.Id != id && x.Name == dto.Name, cancellationToken);

                if (nameExists)
                {
                    throw new InvalidOperationException($"Project with name '{dto.Name}' already exists.");
                }

                project.Name = dto.Name;
            }

            if (dto.CustomerId.HasValue)
            {
                var customerExists = await _dbContext.Customers
                    .AnyAsync(x => x.Id == dto.CustomerId.Value, cancellationToken);

                if (!customerExists)
                {
                    throw new KeyNotFoundException($"Customer with id '{dto.CustomerId.Value}' was not found.");
                }

                project.CustomerId = dto.CustomerId.Value;
            }

            if (dto.ResponsibleEmployeeId.HasValue)
            {
                var employeeExists = await _dbContext.Employees
                    .AnyAsync(x => x.Id == dto.ResponsibleEmployeeId.Value, cancellationToken);

                if (!employeeExists)
                {
                    throw new KeyNotFoundException($"Employee with id '{dto.ResponsibleEmployeeId.Value}' was not found.");
                }

                project.ResponsibleEmployeeId = dto.ResponsibleEmployeeId;
            }

            var nextStartDate = dto.StartDate ?? project.StartDate;
            var nextEndDate = dto.EndDate ?? project.EndDate;

            if (nextEndDate.HasValue && nextEndDate < nextStartDate)
            {
                throw new InvalidOperationException("EndDate cannot be earlier than StartDate.");
            }

            var oldProjectAvatarUrl = project.AvatarUrl;

            project.AvatarUrl = dto.AvatarUrl ?? project.AvatarUrl;
            project.Description = dto.Description ?? project.Description;
            project.Status = dto.Status ?? project.Status;
            project.StartDate = dto.StartDate ?? project.StartDate;
            project.EndDate = dto.EndDate ?? project.EndDate;
            project.UpdatedAt = DateTimeOffset.UtcNow;

            if (dto.Address is not null)
            {
                project.Address.Country = dto.Address.Country;
                project.Address.PostalCode = dto.Address.PostalCode;
                project.Address.City = dto.Address.City;
                project.Address.Street = dto.Address.Street;
                project.Address.BuildingNumber = dto.Address.BuildingNumber;
                project.Address.ApartmentNumber = dto.Address.ApartmentNumber;
                project.Address.FullText = dto.Address.FullText;
                project.Address.AddressType = dto.Address.AddressType;
                project.Address.UpdatedAt = DateTimeOffset.UtcNow;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            if (!string.Equals(oldProjectAvatarUrl, project.AvatarUrl, StringComparison.OrdinalIgnoreCase))
            {
                _fileStorageService.DeleteFileByUrl(oldProjectAvatarUrl);
            }

            var updated = await GetProjectQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToShortDetailsDto();
        }

        public async Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var project = await _dbContext.Projects
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (project is null)
            {
                throw new KeyNotFoundException($"Project with id '{id}' was not found.");
            }

            project.IsDeleted = true;
            project.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }



        public async Task<ProjectNoteListItemDto> CreateNoteAsync(
            CreateProjectNoteDto dto,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var projectExists = await _dbContext.Projects
                .AnyAsync(x => x.Id == dto.ProjectId, ct);

            if (!projectExists)
            {
                throw new KeyNotFoundException($"Project with id '{dto.ProjectId}' was not found.");
            }

            var employeeExists = await _dbContext.Employees
                .AnyAsync(x => x.Id == dto.CreatedByEmployeeId, ct);

            if (!employeeExists)
            {
                throw new KeyNotFoundException($"Employee with id '{dto.CreatedByEmployeeId}' was not found.");
            }

            var now = DateTimeOffset.UtcNow;

            var note = new ProjectNote
            {
                ProjectId = dto.ProjectId,
                Content = dto.Content.Trim(),
                NoteType = dto.NoteType,
                CreatedByEmployeeId = dto.CreatedByEmployeeId,
                CreatedAt = now,
                UpdatedAt = now
            };

            _dbContext.ProjectNotes.Add(note);
            await _dbContext.SaveChangesAsync(ct);

            return new ProjectNoteListItemDto
            {
                Id = note.Id,
                ProjectId = note.ProjectId,
                Content = note.Content,
                NoteType = note.NoteType,
                CreatedByEmployeeId = note.CreatedByEmployeeId,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }

        public async Task<ProjectNoteListItemDto> UpdateNoteAsync(
            long noteId,
            UpdateProjectNoteDto dto,
            long loggedEmployeeId,
            string? role,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var note = await _dbContext.ProjectNotes
                .FirstOrDefaultAsync(x => x.Id == noteId, ct);

            if (note is null)
            {
                throw new KeyNotFoundException($"Project note with id '{noteId}' was not found.");
            }

            var isAdmin = role == "ADMIN";
            var isOwner = note.CreatedByEmployeeId == loggedEmployeeId;

            if (!isAdmin && !isOwner)
                throw new UnauthorizedAccessException("You are not allowed to update this note.");


            var changed = false;

            if (dto.Content is not null)
            {
                note.Content = dto.Content.Trim();
                changed = true;
            }

            if (dto.NoteType.HasValue)
            {
                note.NoteType = dto.NoteType.Value;
                changed = true;
            }

            if (changed)
            {
                note.UpdatedAt = DateTimeOffset.UtcNow;
                await _dbContext.SaveChangesAsync(ct);
            }

            return new ProjectNoteListItemDto
            {
                Id = note.Id,
                ProjectId = note.ProjectId,
                Content = note.Content,
                NoteType = note.NoteType,
                CreatedByEmployeeId = note.CreatedByEmployeeId,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }

        public async Task DeleteNoteAsync(
            long noteId,
            long loggedEmployeeId,
            string? role,
            CancellationToken ct = default)
        {
            var note = await _dbContext.ProjectNotes
                .FirstOrDefaultAsync(x => x.Id == noteId, ct);

            if (note is null)
            {
                throw new KeyNotFoundException($"Project note with id '{noteId}' was not found.");
            }

            var isAdmin = role == "ADMIN";
            var isOwner = note.CreatedByEmployeeId == loggedEmployeeId;

            if (!isAdmin && !isOwner)
                throw new UnauthorizedAccessException("You are not allowed to delete this note.");


            _dbContext.ProjectNotes.Remove(note);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<ProjectTaskListItemDto> CreateTaskAsync(
            long projectId,
            long createdByEmployeeId,
            CreateProjectTaskDto dto,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.ProjectId != projectId)
            {
                throw new InvalidOperationException("ProjectId in route and body must match.");
            }

            var projectExists = await _dbContext.Projects
                .AnyAsync(x => x.Id == projectId, ct);

            if (!projectExists)
            {
                throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
            }

            var createdByEmployee = await _dbContext.Employees
                .Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.Id == createdByEmployeeId, ct);

            if (createdByEmployee is null)
            {
                throw new KeyNotFoundException($"Employee with id '{createdByEmployeeId}' was not found.");
            }

            Employee? assignedEmployee = null;
            if (dto.AssignedToEmployeeId.HasValue)
            {
                assignedEmployee = await _dbContext.Employees
                    .Include(x => x.Person)
                    .FirstOrDefaultAsync(x => x.Id == dto.AssignedToEmployeeId.Value, ct);

                if (assignedEmployee is null)
                {
                    throw new KeyNotFoundException($"Employee with id '{dto.AssignedToEmployeeId.Value}' was not found.");
                }
            }

            var now = DateTimeOffset.UtcNow;

            var task = new ProjectTask
            {
                ProjectId = projectId,
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                Url = dto.Url?.Trim(),
                Caption = dto.Caption?.Trim(),
                Priority = dto.Priority.Trim(),
                Status = dto.Status,
                DueAt = dto.DueAt,
                EstimatedHours = dto.EstimatedHours,
                CreatedByEmployeeId = createdByEmployeeId,
                AssignedToEmployeeId = dto.AssignedToEmployeeId,
                CreatedAt = now,
                UpdatedAt = now
            };

            _dbContext.ProjectTasks.Add(task);
            await _dbContext.SaveChangesAsync(ct);

            return new ProjectTaskListItemDto
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                Title = task.Title,
                Description = task.Description,
                Url = task.Url,
                Caption = task.Caption,
                Priority = task.Priority,
                Status = task.Status,
                DueAt = task.DueAt,
                EstimatedHours = task.EstimatedHours,
                CreatedByEmployeeId = task.CreatedByEmployeeId,
                CreatedByEmployeeName = GetEmployeeDisplayName(createdByEmployee),
                AssignedEmployeeId = assignedEmployee?.Id,
                AssignedEmployeeName = assignedEmployee is null ? null : GetEmployeeDisplayName(assignedEmployee),
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<ProjectTaskListItemDto> UpdateTaskAsync(
            long taskId,
            UpdateProjectTaskDto dto,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var task = await _dbContext.ProjectTasks
                .Include(x => x.CreatedByEmployee)
                    .ThenInclude(x => x.Person)
                .Include(x => x.AssignedToEmployee)
                    .ThenInclude(x => x!.Person)
                .FirstOrDefaultAsync(x => x.Id == taskId, ct);

            if (task is null)
            {
                throw new KeyNotFoundException($"Project task with id '{taskId}' was not found.");
            }

            if (dto.AssignedToEmployeeId.HasValue)
            {
                var assignedExists = await _dbContext.Employees
                    .AnyAsync(x => x.Id == dto.AssignedToEmployeeId.Value, ct);

                if (!assignedExists)
                {
                    throw new KeyNotFoundException($"Employee with id '{dto.AssignedToEmployeeId.Value}' was not found.");
                }

                task.AssignedToEmployeeId = dto.AssignedToEmployeeId.Value;
            }

            if (dto.Title is not null) task.Title = dto.Title.Trim();
            if (dto.Description is not null) task.Description = dto.Description.Trim();
            if (dto.Url is not null) task.Url = dto.Url.Trim();
            if (dto.Caption is not null) task.Caption = dto.Caption.Trim();
            if (dto.Priority is not null) task.Priority = dto.Priority.Trim();
            if (dto.Status.HasValue) task.Status = dto.Status.Value;
            if (dto.DueAt.HasValue) task.DueAt = dto.DueAt;
            if (dto.EstimatedHours.HasValue) task.EstimatedHours = dto.EstimatedHours.Value;

            task.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(ct);

            var refreshedTask = await _dbContext.ProjectTasks
                .AsNoTracking()
                .Include(x => x.CreatedByEmployee)
                    .ThenInclude(x => x.Person)
                .Include(x => x.AssignedToEmployee)
                    .ThenInclude(x => x!.Person)
                .FirstAsync(x => x.Id == taskId, ct);

            return new ProjectTaskListItemDto
            {
                Id = refreshedTask.Id,
                ProjectId = refreshedTask.ProjectId,
                Title = refreshedTask.Title,
                Description = refreshedTask.Description,
                Url = refreshedTask.Url,
                Caption = refreshedTask.Caption,
                Priority = refreshedTask.Priority,
                Status = refreshedTask.Status,
                DueAt = refreshedTask.DueAt,
                EstimatedHours = refreshedTask.EstimatedHours,
                CreatedByEmployeeId = refreshedTask.CreatedByEmployeeId,
                CreatedByEmployeeName = GetEmployeeDisplayName(refreshedTask.CreatedByEmployee),
                AssignedEmployeeId = refreshedTask.AssignedToEmployeeId,
                AssignedEmployeeName = refreshedTask.AssignedToEmployee is null
                    ? null
                    : GetEmployeeDisplayName(refreshedTask.AssignedToEmployee),
                CreatedAt = refreshedTask.CreatedAt
            };
        }

        public async Task DeleteTaskAsync(
            long taskId,
            CancellationToken ct = default)
        {
            var task = await _dbContext.ProjectTasks
                .FirstOrDefaultAsync(x => x.Id == taskId, ct);

            if (task is null)
            {
                throw new KeyNotFoundException($"Project task with id '{taskId}' was not found.");
            }

            _dbContext.ProjectTasks.Remove(task);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<ProjectAttachmentListItemDto> CreateAttachmentAsync(
            CreateProjectAttachmentDto dto,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var projectExists = await _dbContext.Projects
                .AnyAsync(x => x.Id == dto.ProjectId, ct);

            if (!projectExists)
            {
                throw new KeyNotFoundException($"Project with id '{dto.ProjectId}' was not found.");
            }

            var employeeExists = await _dbContext.Employees
                .AnyAsync(x => x.Id == dto.CreatedByEmployeeId, ct);

            if (!employeeExists)
            {
                throw new KeyNotFoundException($"Employee with id '{dto.CreatedByEmployeeId}' was not found.");
            }

            var attachment = new ProjectAttachment
            {
                ProjectId = dto.ProjectId,
                Url = dto.Url.Trim(),
                Caption = dto.Caption?.Trim(),
                AttachmentType = dto.AttachmentType,
                CreatedByEmployeeId = dto.CreatedByEmployeeId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _dbContext.ProjectAttachments.Add(attachment);
            await _dbContext.SaveChangesAsync(ct);

            return new ProjectAttachmentListItemDto
            {
                Id = attachment.Id,
                ProjectId = attachment.ProjectId,
                Url = attachment.Url,
                Caption = attachment.Caption,
                AttachmentType = attachment.AttachmentType,
                CreatedByEmployeeId = attachment.CreatedByEmployeeId,
                CreatedAt = attachment.CreatedAt
            };
        }

        public async Task<ProjectAttachmentListItemDto> UpdateAttachmentAsync(
            long attachmentId,
            UpdateProjectAttachmentDto dto,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var attachment = await _dbContext.ProjectAttachments
                .FirstOrDefaultAsync(x => x.Id == attachmentId, ct);

            if (attachment is null)
            {
                throw new KeyNotFoundException($"Project attachment with id '{attachmentId}' was not found.");
            }

            if (dto.Url is not null) attachment.Url = dto.Url.Trim();
            if (dto.Caption is not null) attachment.Caption = dto.Caption.Trim();
            if (dto.AttachmentType.HasValue) attachment.AttachmentType = dto.AttachmentType.Value;

            await _dbContext.SaveChangesAsync(ct);

            return new ProjectAttachmentListItemDto
            {
                Id = attachment.Id,
                ProjectId = attachment.ProjectId,
                Url = attachment.Url,
                Caption = attachment.Caption,
                AttachmentType = attachment.AttachmentType,
                CreatedByEmployeeId = attachment.CreatedByEmployeeId,
                CreatedAt = attachment.CreatedAt
            };
        }

        public async Task DeleteAttachmentAsync(
            long attachmentId,
            CancellationToken ct = default)
        {
            var attachment = await _dbContext.ProjectAttachments
                .FirstOrDefaultAsync(x => x.Id == attachmentId, ct);

            if (attachment is null)
            {
                throw new KeyNotFoundException($"Project attachment with id '{attachmentId}' was not found.");
            }

            _dbContext.ProjectAttachments.Remove(attachment);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<ProjectSelectOptionDto>> GetProjectAccessSelectOptionsListAsync(
            long loggedEmployeeId,
            CancellationToken ct = default)
        {
           var COS =  await _dbContext.ProjectAccesses
                .AsNoTracking()
                .Where(x => x.EmployeeId == loggedEmployeeId && x.IsEnabled)
                .OrderBy(x => x.Project.Name)
                .Select(x => new ProjectSelectOptionDto
                {
                    ProjectId = x.Project.Id,
                    ProjectName = x.Project.Name,
                    Code = x.Project.Code
                }).ToListAsync(ct);


            return COS;
        }

        public async Task<ProjectAccessListItemDto> CreateAccessAsync(
            CreateProjectAccessDto dto,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var projectExists = await _dbContext.Projects
                .AnyAsync(x => x.Id == dto.ProjectId, ct);

            if (!projectExists)
            {
                throw new KeyNotFoundException($"Project with id '{dto.ProjectId}' was not found.");
            }

            var employee = await _dbContext.Employees
                .Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.Id == dto.EmployeeId, ct);

            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with id '{dto.EmployeeId}' was not found.");
            }

            var accessExists = await _dbContext.ProjectAccesses
                .AnyAsync(x => x.ProjectId == dto.ProjectId && x.EmployeeId == dto.EmployeeId, ct);

            if (accessExists)
            {
                throw new InvalidOperationException(
                    $"Access for employee '{dto.EmployeeId}' in project '{dto.ProjectId}' already exists.");
            }

            var access = new ProjectAccess
            {
                ProjectId = dto.ProjectId,
                EmployeeId = dto.EmployeeId,
                IsEnabled = dto.IsEnabled,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _dbContext.ProjectAccesses.Add(access);
            await _dbContext.SaveChangesAsync(ct);

            return new ProjectAccessListItemDto
            {
                Id = access.Id,
                EmployeeName = GetEmployeeDisplayName(employee),
                IsEnabled = access.IsEnabled,
                CreatedAt = access.CreatedAt
            };
        }

        public async Task<ProjectAccessListItemDto> UpdateAccessAsync(
            long accessId,
            UpdateProjectAccessDto dto,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var access = await _dbContext.ProjectAccesses
                .Include(x => x.Employee)
                    .ThenInclude(x => x.Person)
                .FirstOrDefaultAsync(x => x.Id == accessId, ct);

            if (access is null)
            {
                throw new KeyNotFoundException($"Project access with id '{accessId}' was not found.");
            }

            access.IsEnabled = dto.IsEnabled;

            await _dbContext.SaveChangesAsync(ct);

            return new ProjectAccessListItemDto
            {
                Id = access.Id,
                EmployeeName = GetEmployeeDisplayName(access.Employee),
                IsEnabled = access.IsEnabled,
                CreatedAt = access.CreatedAt
            };
        }

        public async Task DeleteAccessAsync(
            long accessId,
            CancellationToken ct = default)
        {
            var access = await _dbContext.ProjectAccesses
                .FirstOrDefaultAsync(x => x.Id == accessId, ct);

            if (access is null)
            {
                throw new KeyNotFoundException($"Project access with id '{accessId}' was not found.");
            }

            _dbContext.ProjectAccesses.Remove(access);
            await _dbContext.SaveChangesAsync(ct);
        }

        private static string GetEmployeeDisplayName(dynamic employee)
        {
            if (employee?.Person is not null)
            {
                var firstName = employee.Person.FirstName?.Trim();
                var lastName = employee.Person.LastName?.Trim();
                var fullName = $"{firstName} {lastName}".Trim();

                if (!string.IsNullOrWhiteSpace(fullName))
                {
                    return fullName;
                }
            }

            return $"Employee {employee?.Id}";
        }

        public async Task<IReadOnlyList<ProjectSelectOptionDto>> GetSelectOptionsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Projects
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ProjectSelectOptionDto
                {
                    ProjectId = x.Id,
                    ProjectName = x.Name,
                    Code = x.Code
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ProjectTaskListItemDto>> GetTaskListByEmployeeIdAsync(
            long employeeId,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.ProjectTasks
                .AsNoTracking()
                .Include(p => p.Project)
                .Where(p => p.AssignedToEmployeeId == employeeId)
                .OrderBy(p => p.Id);

            return await query
                .Select(p => p.ToTaskListItemDto())
                .ToListAsync();
        }

        public async Task<ProjectTaskListItemDto> GetTaskByIdAsync(
            long taskId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProjectTasks
                .AsNoTracking()
                .Include(p => p.Project)
                .Where(p => p.Id == taskId)
                .Select(p => p.ToTaskListItemDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProjectTaskListItemDto> UpdateStatusAsync(
            long id,
            long loggedEmployeeId,
            UpdateTaskStatusDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var task = await _dbContext.ProjectTasks
                .Include(x => x.CreatedByEmployee)
                    .ThenInclude(x => x.Person)
                .Include(x => x.AssignedToEmployee)
                    .ThenInclude(x => x!.Person)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (task is null)
            {
                throw new KeyNotFoundException($"Project task with id '{id}' was not found.");
            }
            if (task.AssignedToEmployeeId != loggedEmployeeId)
            {
                throw new UnauthorizedAccessException("You are not allowed to update this task status.");
            }

            task.Status = dto.Status;
            task.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return task.ToTaskListItemDto();

        }

        private IQueryable<Project> GetProjectQuery()
        {
            return _dbContext.Projects
                .AsNoTracking()
                .Include(x => x.Address)
                .Include(x => x.Customer)
                    .ThenInclude(x => x.ContactPerson)
                .Include(x => x.ResponsibleEmployee)
                    .ThenInclude(x => x!.Person)
                .Include(x => x.Tasks)
                    .ThenInclude(x => x.CreatedByEmployee)
                    .ThenInclude(x => x.Person)
                .Include(x => x.Tasks)
                    .ThenInclude(x => x.AssignedToEmployee)
                        .ThenInclude(x => x!.Person)
                .Include(x => x.Notes)
                .Include(x => x.Attachments)
                .Include(x => x.WorkSessions)
                    .ThenInclude(x => x.Employee)
                        .ThenInclude(x => x.Person)
                .Include(x => x.WorkSessions)
                .Include(x => x.MaterialMovements)
                    .ThenInclude(x => x.Material)
                .Include(x => x.Accesses)
                    .ThenInclude(x => x.Employee)
                        .ThenInclude(x => x.Person)
                .Include(x => x.Invoices)
                    .ThenInclude(x => x.Customer)
                .Include(x => x.Invoices);
        }
    }
}
