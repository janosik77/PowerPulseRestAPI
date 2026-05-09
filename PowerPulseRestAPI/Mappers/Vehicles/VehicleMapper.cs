using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.VehicleModels;
using PowerPulseRestAPI.DTO.VehicleDto.Responses;

namespace PowerPulseRestAPI.Mappers.Vehicles
{
    public static class VehicleMapper
    {
        public static VehicleListItemDto ToListItemDto(this Vehicle vehicle)
        {
            return new VehicleListItemDto
            {
                Id = vehicle.Id,
                PlateNumber = vehicle.PlateNumber,
                Vin = vehicle.Vin,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Status = vehicle.Status,
                Year = vehicle.Year,
                Url = vehicle.Url
            };
        }

        public static VehicleDetailDto ToDetailDto(this Vehicle vehicle)
        {
            return new VehicleDetailDto
            {
                Id = vehicle.Id,
                PlateNumber = vehicle.PlateNumber,
                Vin = vehicle.Vin,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Url = vehicle.Url,
                Caption = vehicle.Caption,
                Status = vehicle.Status,
                CurrentMileage = vehicle.CurrentMileage,
                LastServiceAt = vehicle.LastServiceAt,
                LastServiceMileage = vehicle.LastServiceMileage,
                UpdatedAt = vehicle.UpdatedAt,
                Assignmens = vehicle.Assignments
                    .OrderByDescending(x => x.AssignedAt)
                    .Select(x => new VehicleAssignmentListItemDto
                    {
                        Id = x.Id,
                        EmployeeId = x.EmployeeId,
                        EmployeeFullName = $"{x.Employee.Person.FirstName} {x.Employee.Person.LastName}",
                        AssignedAt = x.AssignedAt,
                        ReturnedAt = x.ReturnedAt,
                        IsActive = x.ReturnedAt == null,
                        Note = x.Note,
                        CreatedAt = x.CreatedAt
                    })
                    .ToList(),
                Issues = vehicle.Issues
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => x.ToIssueDto())
                    .ToList()
            };
        }

        public static VehicleIssueListItemDto ToIssueDto(this VehicleIssue issue)
        {
            return new VehicleIssueListItemDto
            {
                Id = issue.Id,
                ReportedByUserFullName = issue.ReportedByUser?.Person == null
                    ? "-"
                    : $"{issue.ReportedByUser.Person.FirstName} {issue.ReportedByUser.Person.LastName}",
                Description = issue.Description,
                Status = issue.Status,
                IsResolved = issue.Status == GenericStatus.RESOLVED,
                CreatedAt = issue.CreatedAt,
                UpdatedAt = issue.UpdatedAt
            };
        }
    }
}