using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.DTO.ToolDto.Responses;

namespace PowerPulseRestAPI.Mappers.Tools
{
    public static class ToolMapper
    {
        private static ToolAssignment? GetCurrentAssignment(Tool tool)
        {
            return tool.Assignments
                .FirstOrDefault(a => a.ReturnedAt == null);
        }

        // 🔹 LIST ITEM
        public static ToolListItemDto ToListItemDto(this Tool tool)
        {
            var current = GetCurrentAssignment(tool);

            return new ToolListItemDto
            {
                Id = tool.Id,
                Name = tool.Name,
                Manufacturer = tool.Manufacturer,
                Model = tool.Model,
                Status = tool.Status,
                CategoryName = tool.Category.Name,

                IsAssigned = current != null,
                AssignedEmployeeId = current?.EmployeeId,
                AssignedEmployeeName = current == null
                    ? null
                    : $"{current.Employee.Person.FirstName} {current.Employee.Person.LastName}",
            };
        }

        // 🔹 DETAILS
        public static ToolDetailsDto ToDetailsDto(this Tool tool)
        {
            var current = GetCurrentAssignment(tool);

            return new ToolDetailsDto
            {
                Id = tool.Id,
                Name = tool.Name,
                Description = tool.Description,
                Manufacturer = tool.Manufacturer,
                Model = tool.Model,
                SerialNumber = tool.SerialNumber,
                Url = tool.Url,
                Condition = tool.Condition,
                Status = tool.Status,
                PurchaseDate = tool.PurchaseDate,
                CategoryName = tool.Category.Name,

                IsAssigned = current != null,
                AssignedEmployeeId = current?.EmployeeId,
                AssignedEmployeeName = current == null
                    ? null
                    : $"{current.Employee.Person.FirstName} {current.Employee.Person.LastName}",
                AssignedAt = current?.AssignedAt,

                History = tool.Assignments
                    .OrderByDescending(x => x.AssignedAt)
                    .Select(x => new ToolAssignmentDto
                    {
                        Id = x.Id,
                        EmployeeId = x.EmployeeId,
                        EmployeeName = $"{x.Employee.Person.FirstName} {x.Employee.Person.LastName}",
                        AssignedAt = x.AssignedAt,
                        ReturnedAt = x.ReturnedAt,
                        Notes = x.Notes
                    })
                    .ToList()
            };
        }
    }
}
