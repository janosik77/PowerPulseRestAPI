
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.DTO.InvoiceDto.Responses;
using PowerPulseRestAPI.DTO.MaterialDto.Responses;
using PowerPulseRestAPI.DTO.ProjectDto.Responses;
using PowerPulseRestAPI.DTO.WorkSessionDto.Responses;

namespace PowerPulseRestAPI.Mappers.Projects
{
    public static class ProjectMapper
    {
        public static ProjectListItemDto ToListItemDto(this Project project)
        {
            return new ProjectListItemDto
            {
                Id = project.Id,
                Name = project.Name,
                AvatarUrl = project.AvatarUrl,
                Status = project.Status,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CustomerId = project.CustomerId,
                CustomerName = project.Customer.CompanyName
            };
        }

        public static ProjectShortDetailsDto ToShortDetailsDto(this Project project)
        {
            return new ProjectShortDetailsDto
            {
                Id = project.Id,
                Name = project.Name,
                AvatarUrl = project.AvatarUrl,
                Code = project.Code,
                Description = project.Description,
                Address = BuildAddress(project),
                Status = project.Status,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CustomerName = project.Customer.CompanyName,
                ResponsibleEmployeeId = project.ResponsibleEmployeeId
            };
        }

        public static ProjectFullDetailsDto ToFullDetailsDto(this Project project)
        {
            return new ProjectFullDetailsDto
            {
                Id = project.Id,
                Name = project.Name,
                Address = BuildAddress(project),
                AvatarUrl = project.AvatarUrl,
                Code = project.Code,
                Description = project.Description,
                Status = project.Status,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ContactPersonFullName = $"{project.Customer.ContactPerson.FirstName} {project.Customer.ContactPerson.LastName}" ,
                ContactPersonEmail = project.Customer.ContactPerson.Email,
                ContactPersonPhone = project.Customer.ContactPerson.Phone,
                CustomerName = project.Customer.CompanyName,
                ResponsibleEmployeeId = project.ResponsibleEmployeeId,

                Tasks = project.Tasks
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new ProjectTaskListItemDto
                    {
                        Id = x.Id,
                        ProjectId = x.ProjectId,
                        Title = x.Title,
                        Description = x.Description,
                        Url = x.Url,
                        Caption = x.Caption,
                        Priority = x.Priority,
                        Status = x.Status,
                        DueAt = x.DueAt,
                        EstimatedHours = x.EstimatedHours,
                        CreatedByEmployeeId = x.CreatedByEmployeeId,
                        CreatedByEmployeeName = $"{x.CreatedByEmployee.Person.FirstName} {x.CreatedByEmployee.Person.LastName}",
                        AssignedEmployeeId = x.AssignedToEmployeeId,
                        AssignedEmployeeName = x.AssignedToEmployee == null
                            ? null
                            : $"{x.AssignedToEmployee.Person.FirstName} {x.AssignedToEmployee.Person.LastName}",
                        CreatedAt = x.CreatedAt
                    })
                    .ToList(),

                Notes = project.Notes
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new ProjectNoteListItemDto
                    {
                        Id = x.Id,
                        ProjectId = x.ProjectId,
                        Content = x.Content,
                        NoteType = x.NoteType,
                        CreatedByEmployeeId = x.CreatedByEmployeeId,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                    .ToList(),

                Attachments = project.Attachments
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new ProjectAttachmentListItemDto
                    {
                        Id = x.Id,
                        ProjectId = x.ProjectId,
                        Url = x.Url,
                        Caption = x.Caption,
                        AttachmentType = x.AttachmentType,
                        CreatedByEmployeeId = x.CreatedByEmployeeId,
                        CreatedAt = x.CreatedAt
                    })
                    .ToList(),

                WorkSessions = project.WorkSessions
                    .OrderByDescending(x => x.StartedAt)
                    .Select(x => new WorkSessionListItemDto
                    {
                        Id = x.Id,
                        EmployeeId = x.EmployeeId,
                        EmployeeName = $"{x.Employee.Person.FirstName} {x.Employee.Person.LastName}",
                        ProjectId = x.ProjectId,
                        ProjectName = x.Project.Name,
                        StartedAt = x.StartedAt,
                        EndedAt = x.EndedAt,
                        Status = x.Status,
                        Note = x.Note,
                        InvoiceId = x.InvoiceId,
                        InvoicedAt = x.InvoicedAt
                    })
                    .ToList(),

                MaterialBalance = project.MaterialMovements
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
                        ProjectId = project.Id,
                        MaterialId = g.Key.MaterialId,
                        MaterialName = g.Key.Name,
                        Manufacturer = g.Key.Manufacturer,
                        Url = g.Key.Url,
                        Quantity = g.Sum(x => x.MovementType switch
                        {
                            MaterialMovementType.ISSUE_TO_PROJECT => x.Quantity,
                            MaterialMovementType.PROJECT_CONSUME => -x.Quantity,
                            MaterialMovementType.RETURN_FROM_PROJECT => -x.Quantity,
                            _ => 0m
                        }),
                        Unit = g.Key.Unit
                    })
                    .ToList(),

                Accesses = project.Accesses
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new ProjectAccessListItemDto
                    {
                        Id = x.Id,
                        EmployeeName = $"{x.Employee.Person.FirstName} {x.Employee.Person.LastName}",
                        IsEnabled = x.IsEnabled,
                        CreatedAt = x.CreatedAt
                    })
                    .ToList(),

                Invoices = project.Invoices
                    .OrderByDescending(x => x.IssueDate)
                    .Select(x => new InvoiceListItemDto
                    {
                        Id = x.Id,
                        InvoiceNumber = x.InvoiceNumber,
                        Status = x.Status,
                        ProjectId = x.ProjectId,
                        ProjectName = x.Project?.Name ?? project.Name,
                        CustomerId = x.CustomerId,
                        CustomerName = x.Customer?.CompanyName ?? project.Customer.CompanyName,
                        IssueDate = x.IssueDate,
                        DueDate = x.DueDate,
                        BillingPeriodStart = x.BillingPeriodStart,
                        BillingPeriodEnd = x.BillingPeriodEnd
                    })
                    .ToList()
            };
        }

        private static string BuildAddress(Project project)
        {
            var a = project.Address;

            return string.Join(", ",
                new[]
                {
                    $"{a.Street} {a.BuildingNumber}{(string.IsNullOrWhiteSpace(a.ApartmentNumber) ? "" : $"/{a.ApartmentNumber}")}",
                    a.PostalCode,
                    a.City,
                    a.Country
                }.Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}
