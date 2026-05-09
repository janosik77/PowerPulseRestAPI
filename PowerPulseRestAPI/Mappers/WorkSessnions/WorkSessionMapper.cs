using PowerPulseRestAPI.Data.Models.WorkSessionModels;
using PowerPulseRestAPI.DTO.WorkSessionDto.Responses;

namespace PowerPulseRestAPI.Mappers.WorkSessions
{
    public static class WorkSessionMapper
    {
        public static WorkSessionListItemDto ToListItemDto(this WorkSession workSession)
        {
            return new WorkSessionListItemDto
            {
                Id = workSession.Id,
                EmployeeId = workSession.EmployeeId,
                EmployeeName = $"{workSession.Employee.Person.FirstName} {workSession.Employee.Person.LastName}",
                ProjectId = workSession.ProjectId,
                ProjectName = workSession.Project.Name,
                StartedAt = workSession.StartedAt,
                EndedAt = workSession.EndedAt,
                Status = workSession.Status,
                Note = workSession.Note,
                InvoiceId = workSession.InvoiceId,
                InvoicedAt = workSession.InvoicedAt
            };
        }

        public static EmployeeWorkSessionsListItemDto ToEmployeeListItemDto(this WorkSession workSession)
        {
            return new EmployeeWorkSessionsListItemDto
            {
                Id = workSession.Id,
                ProjectId = workSession.ProjectId,
                ProjectCode = workSession.Project.Code,
                ProjectName = workSession.Project.Name,
                StartedAt = workSession.StartedAt,
                EndedAt = workSession.EndedAt,
                Status = workSession.Status
            };
        }
    }
}