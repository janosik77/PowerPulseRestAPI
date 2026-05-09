using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.DTO.ProjectDto.Responses;

namespace PowerPulseRestAPI.Mappers.Projects
{
    public static class ProjectTaskMapper
    {
        public static ProjectTaskListItemDto ToTaskListItemDto(this ProjectTask task)
        {
            return new ProjectTaskListItemDto
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                ProjectName = task.Project.Name,
                Title = task.Title,
                Description = task.Description,
                Url = task.Url,
                Caption = task.Caption,
                Priority = task.Priority,
                Status = task.Status,
                DueAt = task.DueAt,
                EstimatedHours = task.EstimatedHours,
                CreatedByEmployeeId = task.CreatedByEmployeeId,
                CreatedByEmployeeName = task.CreatedByEmployee?.Person == null
                    ? $"Employee {task.CreatedByEmployeeId}"
                    : $"{task.CreatedByEmployee.Person.FirstName} {task.CreatedByEmployee.Person.LastName}".Trim(),
                AssignedEmployeeId = task.AssignedToEmployeeId,
                AssignedEmployeeName = task.AssignedToEmployee?.Person == null
                    ? null
                    : $"{task.AssignedToEmployee.Person.FirstName} {task.AssignedToEmployee.Person.LastName}".Trim(),
                CreatedAt = task.CreatedAt
            };
        }
    }
}
