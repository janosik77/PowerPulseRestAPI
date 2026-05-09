using PowerPulseRestAPI.Data.Models.AddressModels;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.DTO.EmployeeDto.Requests;
using PowerPulseRestAPI.DTO.EmployeeDto.Responses;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.PersonModels;
using PowerPulseRestAPI.Mappers.Employees;
using PowerPulseRestAPI.Services.Security;
using PowerPulseRestAPI.Services.Uploads;

namespace PowerPulseRestAPI.Services.Employees
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly PowerPulseContext _dbContext;
        //private readonly IPasswordHasher<User> _hasher;
        private readonly IEncryptionService _encryptionService;
        private readonly IFileStorageService _fileStorageService;

        public EmployeeService(
            PowerPulseContext dbContext,
            IEncryptionService encryptionService,
            IFileStorageService fileStorageService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
            _fileStorageService = fileStorageService;
        }

        public async Task<EmployeeDetailPrivateDto> CreateAsync(
            CreateEmployeeDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var emailExists = await _dbContext.Persons
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Email == dto.Person.Email, cancellationToken);

            if (emailExists)
            {
                throw new InvalidOperationException($"Person with email '{dto.Person.Email}' already exists.");
            }

            var userEmailExists = await _dbContext.Users
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Email == dto.User.Email, cancellationToken);

            if (userEmailExists)
            {
                throw new InvalidOperationException($"User with email '{dto.User.Email}' already exists.");
            }

            var loginExists = await _dbContext.Users
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Login == dto.User.Login, cancellationToken);

            if (loginExists)
            {
                throw new InvalidOperationException($"User with login '{dto.User.Login}' already exists.");
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            var now = DateTimeOffset.UtcNow;

            var address = new Address
            {
                Country = dto.Person.Address.Country,
                PostalCode = dto.Person.Address.PostalCode,
                City = dto.Person.Address.City,
                Street = dto.Person.Address.Street,
                BuildingNumber = dto.Person.Address.BuildingNumber,
                ApartmentNumber = dto.Person.Address.ApartmentNumber,
                FullText = dto.Person.Address.FullText,
                AddressType = dto.Person.Address.AddressType,
                CreatedAt = now,
                UpdatedAt = now
            };

            _dbContext.Addresses.Add(address);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var person = new Person
            {
                FirstName = dto.Person.FirstName,
                LastName = dto.Person.LastName,
                Phone = dto.Person.Phone,
                DateOfBirth = dto.Person.DateOfBirth!,
                AvatarUrl = dto.Person.AvatarUrl,
                Email = dto.Person.Email,
                AddressId = address.Id,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false,
            };

            _dbContext.Persons.Add(person);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.User.Password);

            var user = new User
            {
                Email = dto.User.Email,
                Login = dto.User.Login,
                PasswordHash = passwordHash,
                RoleId = dto.User.RoleId,
                PersonId = person.Id,
                IsActive = dto.User.IsActive,
                CreatedAt = now,
                UpdatedAt = now,
                LastPasswordUpdate = now,
                IsDeleted = false,

            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var employee = new Employee
            {
                PersonId = person.Id,
                HireDate = dto.Employment.HireDate,
                Department = dto.Employment.Department,
                JobTitle = dto.Employment.JobTitle,
                HourlyWage = dto.Employment.HourlyWage,
                Currency = dto.Employment.Currency,
                Status = dto.Employment.Status,
                RemainingVacationDays = dto.Employment.RemainingVacationDays,
                VacationDaysPerYear = dto.Employment.VacationDaysPerYear,
                AccountEncrypted = _encryptionService.Encrypt(dto.Employment.AccountNumber),
                AccountLast4 = _encryptionService.GetLast4(dto.Employment.AccountNumber),
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false,
            };

            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var identifier = new PersonIdentifier
            {
                PersonId = person.Id,
                EncryptedSSN = _encryptionService.Encrypt(dto.Identifier.Value),
                Last4 = _encryptionService.GetLast4(dto.Identifier.Value),
                CreatedAt = now,
                UpdatedAt = now
            };

            _dbContext.PersonIdentifiers.Add(identifier);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            var created = await GetEmployeeQuery()
                .FirstAsync(x => x.Id == employee.Id, cancellationToken);

            var createdIdentifier = created.Person.Identifiers.FirstOrDefault();

            if (createdIdentifier is null)
            {
                throw new InvalidOperationException("Employee identifier was not found.");
            }

            var decryptedIdentifier = _encryptionService.Decrypt(createdIdentifier.EncryptedSSN);
            var decryptedAccountNumber = _encryptionService.Decrypt(created.AccountEncrypted);

            return created.ToPrivateDetailsDto(decryptedIdentifier, decryptedAccountNumber);
        }

        public async Task<IReadOnlyList<EmployeeListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default)
        {
            var employees = await GetEmployeeQuery()
                .OrderBy(x => x.Person.LastName)
                .ThenBy(x => x.Person.FirstName)
                .ToListAsync(cancellationToken);

            return employees
                .Select(x => x.ToListItemDto())
                .ToList();
        }

        public async Task<EmployeeDetailsPublicDto> GetPublicByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var employee = await GetEmployeeQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with id '{id}' was not found.");
            }

            return employee.ToPublicDetailsDto();
        }

        public async Task<EmployeeDetailPrivateDto> GetPrivateByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var employee = await GetEmployeeQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (employee is null)
            {
                throw new InvalidOperationException("Employee identifier was not found.");
            }

            var identifier = employee.Person.Identifiers.FirstOrDefault();

            

            var decryptedIdentifier = _encryptionService.Decrypt(identifier.EncryptedSSN);
            var decryptedAccountNumber = _encryptionService.Decrypt(employee.AccountEncrypted);

            return employee.ToPrivateDetailsDto(decryptedIdentifier, decryptedAccountNumber);
        }

        public async Task<EmployeeDetailPrivateDto> UpdateAsync(
            long id,
            UpdateEmployeeDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var employee = await _dbContext.Employees
                .Include(x => x.Person)
                    .ThenInclude(x => x.Address)
                .Include(x => x.Person)
                    .ThenInclude(x => x.User)
                        .ThenInclude(x => x.Role)
                .Include(x => x.Person)
                    .ThenInclude(x => x.Identifiers)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with id '{id}' was not found.");
            }

            var emailExists = await _dbContext.Persons
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Id != employee.PersonId && x.Email == dto.Person.Email, cancellationToken);

            if (emailExists)
            {
                throw new InvalidOperationException($"Person with email '{dto.Person.Email}' already exists.");
            }

            var userEmailExists = await _dbContext.Users
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Id != employee.Person.User!.Id && x.Email == dto.User.Email, cancellationToken);

            if (userEmailExists)
            {
                throw new InvalidOperationException($"User with email '{dto.User.Email}' already exists.");
            }

            var loginExists = await _dbContext.Users
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Id != employee.Person.User!.Id && x.Login == dto.User.Login, cancellationToken);

            if (loginExists)
            {
                throw new InvalidOperationException($"User with login '{dto.User.Login}' already exists.");
            }

            var oldPersonAvatarUrl = employee.Person.AvatarUrl;

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            var now = DateTimeOffset.UtcNow;

            employee.Person.FirstName = dto.Person.FirstName;
            employee.Person.LastName = dto.Person.LastName;
            employee.Person.Phone = dto.Person.Phone;
            employee.Person.DateOfBirth = dto.Person.DateOfBirth!.Value;
            employee.Person.AvatarUrl = dto.Person.AvatarUrl;
            employee.Person.Email = dto.Person.Email;
            employee.Person.UpdatedAt = now;

            employee.Person.Address!.Country = dto.Person.Address.Country;
            employee.Person.Address.PostalCode = dto.Person.Address.PostalCode;
            employee.Person.Address.City = dto.Person.Address.City;
            employee.Person.Address.Street = dto.Person.Address.Street;
            employee.Person.Address.BuildingNumber = dto.Person.Address.BuildingNumber;
            employee.Person.Address.ApartmentNumber = dto.Person.Address.ApartmentNumber;
            employee.Person.Address.FullText = dto.Person.Address.FullText;
            employee.Person.Address.AddressType = dto.Person.Address.AddressType;
            employee.Person.Address.UpdatedAt = now;

            employee.Person.User!.Email = dto.User.Email;
            employee.Person.User.Login = dto.User.Login;
            employee.Person.User.RoleId = dto.User.RoleId;
            employee.Person.User.IsActive = dto.User.IsActive;
            employee.Person.User.UpdatedAt = now;

            if (!string.IsNullOrWhiteSpace(dto.User.NewPassword))
            {
                employee.Person.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.User.NewPassword);
                employee.Person.User.LastPasswordUpdate = now;
            }

            employee.HireDate = dto.Employment.HireDate;
            employee.TerminatedAt = dto.Employment.TerminatedAt;
            employee.Department = dto.Employment.Department;
            employee.JobTitle = dto.Employment.JobTitle;
            employee.HourlyWage = dto.Employment.HourlyWage;
            employee.Currency = dto.Employment.Currency;
            employee.Status = dto.Employment.Status;
            employee.RemainingVacationDays = dto.Employment.RemainingVacationDays;
            employee.VacationDaysPerYear = dto.Employment.VacationDaysPerYear;
            employee.AccountEncrypted = _encryptionService.Encrypt(dto.Employment.AccountNumber);
            employee.AccountLast4 = _encryptionService.GetLast4(dto.Employment.AccountNumber);
            employee.UpdatedAt = now;

            var identifier = employee.Person.Identifiers.FirstOrDefault();
            if (identifier is null)
            {
                identifier = new PersonIdentifier
                {
                    PersonId = employee.PersonId,
                    EncryptedSSN = _encryptionService.Encrypt(dto.Identifier.Value),
                    Last4 = _encryptionService.GetLast4(dto.Identifier.Value),
                    CreatedAt = now,
                    UpdatedAt = now
                };

                _dbContext.PersonIdentifiers.Add(identifier);
            }
            else
            {
                identifier.EncryptedSSN = _encryptionService.Encrypt(dto.Identifier.Value);
                identifier.Last4 = _encryptionService.GetLast4(dto.Identifier.Value);
                identifier.UpdatedAt = now;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            if (!string.Equals(oldPersonAvatarUrl, employee.Person.AvatarUrl, StringComparison.OrdinalIgnoreCase))
            {
                _fileStorageService.DeleteFileByUrl(oldPersonAvatarUrl);
            }
                
            var updated = await GetEmployeeQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            PersonIdentifier? updatedIdentifier = updated.Person.Identifiers.FirstOrDefault();

            if (updatedIdentifier is null)
            {
                throw new InvalidOperationException("Employee identifier was not found.");
            }

            var decryptedIdentifier = _encryptionService.Decrypt(updatedIdentifier.EncryptedSSN);
            var decryptedAccountNumber = _encryptionService.Decrypt(updated.AccountEncrypted);

            return updated.ToPrivateDetailsDto(decryptedIdentifier, decryptedAccountNumber);
        }

        public async Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var employee = await _dbContext.Employees
                .Include(x => x.Person)
                    .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (employee is null)
            {
                throw new KeyNotFoundException($"Employee with id '{id}' was not found.");
            }

            var now = DateTimeOffset.UtcNow;

            employee.IsDeleted = true;
            employee.UpdatedAt = now;

            employee.Person.IsDeleted = true;
            employee.Person.UpdatedAt = now;

            if (employee.Person.User is not null)
            {
                employee.Person.User.IsDeleted = true;
                employee.Person.User.UpdatedAt = now;
                employee.Person.User.IsActive = false;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<Employee> GetEmployeeQuery()
        {
            return _dbContext.Employees
                .AsNoTracking()
                .Include(x => x.Person)
                    .ThenInclude(x => x.Address)
                .Include(x => x.Person)
                    .ThenInclude(x => x.User)
                        .ThenInclude(x => x.Role)
                .Include(x => x.Person)
                    .ThenInclude(x => x.Identifiers);
        }

        public async  Task<IReadOnlyList<SelectOptionsEmployeeDto>> GetSelectOptionsAsync(CancellationToken cancellationToken = default)
        {
            var employee = await _dbContext.Employees
                .AsNoTracking()
                .Include(x => x.Person)
                .Select(x => new SelectOptionsEmployeeDto
                {
                    Value = x.Id,
                    Label = $"{x.Person.FirstName} {x.Person.LastName}"
                })
                .ToListAsync(cancellationToken);

            return employee;
        }
    }
}
