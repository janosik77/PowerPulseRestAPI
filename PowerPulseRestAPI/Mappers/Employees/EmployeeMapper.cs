using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.DTO.EmployeeDto.Responses;
using PowerPulseRestAPI.DTO.PersonDto.Responses;
using PowerPulseRestAPI.DTO.UsersDto.Responses;
using PowerPulseRestAPI.Mappers.Addresses;

namespace PowerPulseRestAPI.Mappers.Employees
{
    public static class EmployeeMapper
    {
        public static EmployeeListItemDto ToListItemDto(this Employee employee)
        {
            return new EmployeeListItemDto
            {
                Id = employee.Id,
                FullName = $"{employee.Person.FirstName} {employee.Person.LastName}",
                Phone = employee.Person.Phone,
                JobTitle = employee.JobTitle,
                Status = employee.Status,
                AvatarUrl = employee.Person.AvatarUrl ?? string.Empty,
                IsActive = employee.Person.User?.IsActive ?? false
            };
        }

        public static EmployeeDetailsPublicDto ToPublicDetailsDto(this Employee employee)
        {
            return new EmployeeDetailsPublicDto
            {
                Id = employee.Id,
                FirstName = employee.Person.FirstName,
                LastName = employee.Person.LastName,
                Phone = employee.Person.Phone,
                AvatarUrl = employee.Person.AvatarUrl,
                Email = employee.Person.Email,
                HireDate = employee.HireDate,
                TerminatedAt = employee.TerminatedAt,
                Department = employee.Department,
                JobTitle = employee.JobTitle,
                Status = employee.Status
            };
        }

        public static EmployeeDetailPrivateDto ToPrivateDetailsDto(this Employee employee, string decryptedIdentifier,
            string decryptedAccountNumber)
        {
            return new EmployeeDetailPrivateDto
            {
                Id = employee.Id,
                Person = new EmployeePersonDto
                {
                    Id = employee.Person.Id,
                    FirstName = employee.Person.FirstName,
                    LastName = employee.Person.LastName,
                    Phone = employee.Person.Phone,
                    DateOfBirth = employee.Person.DateOfBirth,
                    AvatarUrl = employee.Person.AvatarUrl ?? string.Empty,
                    Email = employee.Person.Email,
                    Address = employee.Person.Address!.ToDto()
                },
                User = new EmployeeUserDto
                {
                    Id = employee.Person.User!.Id,
                    Email = employee.Person.User.Email,
                    Login = employee.Person.User.Login,
                    RoleId = employee.Person.User.RoleId,
                    RoleName = employee.Person.User.Role.Name,
                    IsActive = employee.Person.User.IsActive,
                    LastPasswordUpdate = employee.Person.User.LastPasswordUpdate,
                    LastLoginAt = employee.Person.User.LastLoginAt
                },
                Employment = new EmployeeEmploymentDto
                {
                    Id = employee.Id,
                    HireDate = employee.HireDate,
                    TerminatedAt = employee.TerminatedAt,
                    Department = employee.Department,
                    JobTitle = employee.JobTitle,
                    HourlyWage = employee.HourlyWage,
                    Currency = employee.Currency,
                    Status = employee.Status,
                    RemainingVacationDays = employee.RemainingVacationDays,
                    VacationDaysPerYear = employee.VacationDaysPerYear,
                    AccountNumber = decryptedAccountNumber
                },
                Identifier = new EmployeeIdentifierDto
                {
                    Id = employee.Person.Identifiers.First().Id,
                    SSN = decryptedIdentifier
                }
            };
        }
    }
}
