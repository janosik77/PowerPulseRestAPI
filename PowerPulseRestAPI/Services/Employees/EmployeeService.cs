using Microsoft.AspNetCore.Identity;
using PowerPulseRestAPI.Data.Models.AddressModels;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.DTO.EmployeeDto.Requests;
using PowerPulseRestAPI.DTO.EmployeeDto.Responses;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Security;
using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Services.Employees
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly PowerPulseContext _db;
        private readonly IPasswordHasher<User> _hasher;
        private readonly ISecretProtector _protector;

        public EmployeeService(PowerPulseContext db, IPasswordHasher<User> hasher, ISecretProtector protector)
        {
            _db = db;
            _hasher = hasher;
            _protector = protector;
        }

        public async Task<IReadOnlyList<EmployeeCardDto>> GetCardsAsync(CancellationToken ct)
        {
            return await _db.Set<Employee>()
                .AsNoTracking()
                .OrderBy(e => e.Person!.LastName)
                .Select(e => new EmployeeCardDto
                {
                    Id = e.Id,
                    FirstName = e.Person!.FirstName,
                    LastName = e.Person!.LastName,
                    AvatarUrl = e.Person!.AvatarUrl,
                    JobTitle = e.JobTitle,
                    Status = e.Status,
                    IsWorkingNow = _db.Set<WorkSession>().Any(ws => ws.EmployeeId == e.Id && ws.EndedAt == null),
                    CurrentProject = _db.Set<WorkSession>()
                        .Where(ws => ws.EmployeeId == e.Id && ws.EndedAt == null)
                        .OrderByDescending(ws => ws.StartedAt)
                        .Select(ws => new CurrentProjectDto
                        {
                            WorkSessionId = ws.Id,
                            ProjectId = ws.ProjectId,
                            Code = ws.Project!.Code,
                            Name = ws.Project!.Name,
                            StartedAt = ws.StartedAt,
                            Status = ws.Status
                        })
                        .FirstOrDefault()
                })
                .ToListAsync(ct);
        }

        public Task<long?> GetEmployeeUserIdAsync(long employeeId, CancellationToken ct)
            => _db.Set<Employee>()
                .AsNoTracking()
                .Where(e => e.Id == employeeId)
                .Select(e => (long?)e.Person!.UserId)
                .FirstOrDefaultAsync(ct);

        public async Task<EmployeeDetailsPublicDto?> GetPublicDetailsAsync(long employeeId, CancellationToken ct)
        {
            var e = await _db.Set<Employee>()
                .AsNoTracking()
                .Where(x => x.Id == employeeId)
                .Select(x => new
                {
                    x.Id,
                    x.PersonId,
                    Person = x.Person!,
                    x.JobTitle,
                    x.Department,
                    x.EmployeeType,
                    x.Status
                })
                .FirstOrDefaultAsync(ct);

            if (e is null) return null;

            return new EmployeeDetailsPublicDto
            {
                Id = e.Id,
                PersonId = e.PersonId,
                FirstName = e.Person.FirstName,
                LastName = e.Person.LastName,
                AvatarUrl = e.Person.AvatarUrl,
                JobTitle = e.JobTitle,
                Department = e.Department,
                EmployeeType = e.EmployeeType,
                Status = e.Status,
                CurrentProject = await GetCurrentProjectAsync(employeeId, ct),
                CurrentVehicle = await GetCurrentVehicleAsync(employeeId, ct),
                Tools = await GetCurrentEmployeeToolsAsync(employeeId, ct)
            };
        }

        public async Task<EmployeeDetailsPrivateDto?> GetPrivateDetailsAsync(long employeeId, CancellationToken ct)
        {
            var e = await _db.Set<Employee>()
                .AsNoTracking()
                .Where(x => x.Id == employeeId)
                .Select(x => new
                {
                    Emp = x,
                    Person = x.Person!,
                    User = x.Person!.User!,
                    RoleName = x.Person!.User!.Role!.Name
                })
                .FirstOrDefaultAsync(ct);

            if (e is null) return null;

            var addresses = await GetAddressesInternalAsync(e.Person.Id, ct);

            var bankAccounts = await _db.Set<EmployeeBankAccount>()
                .AsNoTracking()
                .Where(b => b.EmployeeId == employeeId)
                .OrderByDescending(b => b.IsPrimary)
                .ThenByDescending(b => b.UpdatedAt)
                .Select(b => new EmployeeBankAccountDto
                {
                    Id = b.Id,
                    IsPrimary = b.IsPrimary,
                    Country = b.Country,
                    AccountLast4 = b.AccountLast4,
                    ValidFrom = b.ValidFrom,
                    ValidTo = b.ValidTo,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt
                })
                .ToListAsync(ct);

            return new EmployeeDetailsPrivateDto
            {
                Id = e.Emp.Id,
                PersonId = e.Person.Id,
                UserId = e.User.Id,
                RoleName = e.RoleName,

                Email = e.User.Email,
                Login = e.User.Login,
                IsUserActive = e.User.IsActive,
                LastLoginAt = e.User.LastLoginAt,
                LastPasswordUpdate = e.User.LastPasswordUpdate,

                FirstName = e.Person.FirstName,
                LastName = e.Person.LastName,
                AvatarUrl = e.Person.AvatarUrl,
                Phone = e.Person.Phone,
                DateOfBirth = e.Person.DateOfBirth,

                JobTitle = e.Emp.JobTitle,
                Department = e.Emp.Department,
                EmployeeType = e.Emp.EmployeeType,
                Status = e.Emp.Status,

                HireDate = e.Emp.HireDate,
                TerminatedAt = e.Emp.TerminatedAt,
                RemainingVacationDays = e.Emp.RemainingVacationDays,
                VacationDaysPerYear = e.Emp.VacationDaysPerYear,

                CurrentProject = await GetCurrentProjectAsync(employeeId, ct),
                CurrentVehicle = await GetCurrentVehicleAsync(employeeId, ct),
                Tools = await GetCurrentEmployeeToolsAsync(employeeId, ct),

                Addresses = addresses,
                BankAccounts = bankAccounts
            };
        }

        public async Task<IReadOnlyList<EmployeeLeaveDto>> GetLeavesAsync(long employeeId, DateOnly from, DateOnly to, CancellationToken ct)
        {
            return await _db.Set<LeaveRequest>()
                .AsNoTracking()
                .Where(l => l.EmployeeId == employeeId && l.EndDate >= from && l.StartDate <= to)
                .OrderBy(l => l.StartDate)
                .Select(l => new EmployeeLeaveDto
                {
                    Id = l.Id,
                    LeaveType = l.LeaveType,
                    Status = l.Status,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Reason = l.Reason,
                    RequestedByUserId = l.RequestedByUserId,
                    ApprovedByUserId = l.ApprovedByUserId,
                    ApprovedAt = l.ApprovedAt
                })
                .ToListAsync(ct);
        }

        public async Task<long> CreateAsync(EmployeeCreateRequest req, long managerUserId, CancellationToken ct)
        {
            var now = DateTimeOffset.UtcNow;

            await using var tx = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                var conflict = await _db.Set<User>()
                    .AsNoTracking()
                    .AnyAsync(u => u.Email == req.Email || u.Login == req.Login, ct);

                if (conflict) throw new InvalidOperationException("User with given email/login already exists.");

                var user = new User
                {
                    Email = req.Email,
                    Login = req.Login,
                    IsActive = req.IsUserActive,
                    CreatedAt = now,
                    UpdatedAt = now,
                    RoleId = req.RoleId
                };

                user.PasswordHash = _hasher.HashPassword(user, req.TemporaryPassword);
                _db.Add(user);
                await _db.SaveChangesAsync(ct);

                var person = new Person
                {
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Phone = req.Phone,
                    DateOfBirth = req.DateOfBirth,
                    AvatarUrl = req.AvatarUrl,
                    UserId = user.Id,
                    CreatedAt = now,
                    UpdatedAt = now,
                };
                _db.Add(person);
                await _db.SaveChangesAsync(ct);

                var employee = new Employee
                {
                    JobTitle = req.JobTitle,
                    Department = req.Department,
                    HireDate = req.HireDate,
                    TerminatedAt = null,
                    EmployeeType = req.EmployeeType,
                    Status = req.Status,
                    RemainingVacationDays = req.RemainingVacationDays,
                    VacationDaysPerYear = req.VacationDaysPerYear,
                    Person = person
                };

                _db.Add(employee);
                await _db.SaveChangesAsync(ct);

                var address = new Address
                {
                    Country = req.Address.Country,
                    PostalCode = req.Address.PostalCode,
                    City = req.Address.City,
                    Street = req.Address.Street,
                    BuildingNumber = req.Address.BuildingNumber,
                    ApartmentNumber = req.Address.ApartmentNumber,
                    FullText = req.Address.Country + req.Address.PostalCode + req.Address.City + req.Address.Street + req.Address.BuildingNumber + "/" + req.Address.ApartmentNumber,
                    Latitude = req.Address.Latitude,
                    Longitude = req.Address.Longitude,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                _db.Add(address);
                await _db.SaveChangesAsync(ct);

                var link = new EntityAddress
                {
                    AddressId = address.Id,
                    EntityType = EntityType.PERSON,
                    EntityId = person.Id,     
                    AddressType = AddressType.MAIN,
                    IsPrimary = true,
                    CreatedAt = now
                };
                _db.Add(link);

                var encrypted = _protector.Encrypt(req.BankAccount.AccountPlain);
                var last4 = req.BankAccount.AccountPlain.Length >= 4
                    ? req.BankAccount.AccountPlain[^4..]
                    : null;

                var bank = new EmployeeBankAccount
                {
                    EmployeeId = employee.Id,
                    AccountEncrypted = encrypted,
                    AccountLast4 = last4,
                    Country = req.BankAccount.Country,
                    IsPrimary = req.BankAccount.IsPrimary,
                    ValidFrom = req.BankAccount.ValidFrom,
                    ValidTo = req.BankAccount.ValidTo,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                _db.Add(bank);

                await _db.SaveChangesAsync(ct);

                await tx.CommitAsync(ct);
                return employee.Id;
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long employeeId, EmployeeUpdateRequest req, CancellationToken ct)
        {
            var emp = await _db.Set<Employee>()
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == employeeId, ct);

            if (emp is null || emp.Person is null) return false;

            emp.Person.FirstName = req.FirstName;
            emp.Person.LastName = req.LastName;
            emp.Person.Phone = req.Phone;
            emp.Person.DateOfBirth = req.DateOfBirth;
            emp.Person.AvatarUrl = req.AvatarUrl;
            emp.Person.UpdatedAt = DateTimeOffset.UtcNow;

            emp.JobTitle = req.JobTitle;
            emp.Department = req.Department;
            emp.EmployeeType = req.EmployeeType;
            emp.Status = req.Status;
            emp.RemainingVacationDays = req.RemainingVacationDays;
            emp.VacationDaysPerYear = req.VacationDaysPerYear;
            emp.TerminatedAt = req.TerminatedAt;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> TerminateAsync(long employeeId, EmployeeTerminateRequest req, CancellationToken ct)
        {
            var emp = await _db.Set<Employee>().FirstOrDefaultAsync(e => e.Id == employeeId, ct);
            if (emp is null) return false;

            emp.TerminatedAt = req.TerminatedAt;
            if (req.NewStatus.HasValue) emp.Status = req.NewStatus.Value;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        // VEHICLES
        public async Task<bool> AssignVehicleAsync(long employeeId, EmployeeAssignVehicleRequest req, long managerUserId, CancellationToken ct)
        {
            var empExists = await _db.Set<Employee>().AnyAsync(e => e.Id == employeeId, ct);
            var vehicleExists = await _db.Set<Vehicle>().AnyAsync(v => v.Id == req.VehicleId, ct);
            if (!empExists || !vehicleExists) return false;

            var now = DateTimeOffset.UtcNow;

            var current = await _db.Set<VehicleAssignment>()
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.ReturnedAt == null, ct);

            if (current != null) current.ReturnedAt = now;

            _db.Add(new VehicleAssignment
            {
                EmployeeId = employeeId,
                VehicleId = req.VehicleId,
                AssignedAt = now,
                ReturnedAt = null,
                Note = req.Note,
                CreatedAt = now
            });

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DetachCurrentVehicleAsync(long employeeId, long managerUserId, CancellationToken ct)
        {
            var active = await _db.Set<VehicleAssignment>()
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.ReturnedAt == null, ct);

            if (active is null) return false;

            active.ReturnedAt = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // TOOLS
        public async Task<bool> AssignToolAsync(long employeeId, EmployeeAssignToolRequest req, long managerUserId, CancellationToken ct)
        {
            var empExists = await _db.Set<Employee>().AnyAsync(e => e.Id == employeeId, ct);
            var asset = await _db.Set<ToolAsset>().FirstOrDefaultAsync(t => t.Id == req.ToolAssetId, ct);
            if (!empExists || asset is null) return false;

            var alreadyAssigned = await _db.Set<ToolAssignment>()
                .AnyAsync(a => a.ToolAssetId == req.ToolAssetId && a.ReturnedAt == null, ct);

            if (alreadyAssigned) throw new InvalidOperationException("ToolAsset is already assigned.");

            var now = DateTimeOffset.UtcNow;

            _db.Add(new ToolAssignment
            {
                ToolAssetId = req.ToolAssetId,
                ToEmployeeId = employeeId,
                AssignedAt = now,
                ReturnedAt = null,
                Notes = req.Notes,
                CreatedByUserId = managerUserId,
                CreatedAt = now
            });

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<int> ReturnAllToolsAsync(long employeeId, long managerUserId, CancellationToken ct)
        {
            var list = await _db.Set<ToolAssignment>()
                .Where(a => a.ToEmployeeId == employeeId && a.ReturnedAt == null)
                .ToListAsync(ct);

            if (list.Count == 0) return 0;

            var now = DateTimeOffset.UtcNow;
            foreach (var a in list) a.ReturnedAt = now;

            await _db.SaveChangesAsync(ct);
            return list.Count;
        }

        // SESSIONS
        public async Task<bool> ForceEndActiveWorkSessionAsync(long employeeId, long managerUserId, CancellationToken ct)
        {
            var ws = await _db.Set<WorkSession>()
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.EndedAt == null, ct);

            if (ws is null) return false;

            ws.EndedAt = DateTimeOffset.UtcNow;
            ws.UpdatedAt = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // ADDRESSES
        public async Task<IReadOnlyList<EmployeeAddressDto>> GetAddressesAsync(long employeeId, CancellationToken ct)
        {
            var personId = await _db.Set<Employee>()
                .AsNoTracking()
                .Where(e => e.Id == employeeId)
                .Select(e => (long?)e.PersonId)
                .FirstOrDefaultAsync(ct);

            if (personId is null) return Array.Empty<EmployeeAddressDto>();

            return await GetAddressesInternalAsync(personId.Value, ct);
        }

        public async Task<long> AddAddressAsync(
                                                long employeeId,
                                                EmployeeAddressUpsertRequest req,
                                                long managerUserId,
                                                CancellationToken ct)
        {
            var personId = await _db.Set<Employee>()
                .AsNoTracking()
                .Where(e => e.Id == employeeId)
                .Select(e => (long?)e.PersonId)
                .FirstOrDefaultAsync(ct);

            if (personId is null)
                throw new InvalidOperationException("Employee not found.");

            var now = DateTimeOffset.UtcNow;

            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            if (req.IsPrimary)
            {
                // reset w obrębie (PersonId, AddressType)
                await _db.Set<EntityAddress>()
                    .Where(ea =>
                        ea.EntityType == EntityType.PERSON &&
                        ea.EntityId == personId.Value &&
                        ea.AddressType == req.AddressType &&
                        ea.IsPrimary)
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsPrimary, false), ct);
                // ExecuteUpdateAsync = EF Core 7+. Jeśli nie masz, zostaw ToList+foreach.
            }

            var addr = new Address
            {
                Country = req.Country,
                PostalCode = req.PostalCode,
                City = req.City,
                Street = req.Street,
                BuildingNumber = req.BuildingNumber,
                ApartmentNumber = req.ApartmentNumber,
                FullText = req.FullText,
                CreatedAt = now,
                UpdatedAt = now
            };

            var eaNew = new EntityAddress
            {
                Address = addr,              // <- ważne: nawigacja zamiast AddressId
                EntityType = EntityType.PERSON,
                EntityId = personId.Value,
                AddressType = req.AddressType,
                IsPrimary = req.IsPrimary,
                CreatedAt = now
            };

            _db.Add(eaNew);
            await _db.SaveChangesAsync(ct);

            await tx.CommitAsync(ct);

            return eaNew.Id;
        }

        public async Task<bool> UpdateAddressAsync(long employeeId, long entityAddressId, EmployeeAddressUpsertRequest req, long managerUserId, CancellationToken ct)
        {
            var personId = await _db.Set<Employee>()
                .AsNoTracking()
                .Where(e => e.Id == employeeId)
                .Select(e => (long?)e.PersonId)
                .FirstOrDefaultAsync(ct);

            if (personId is null) return false;

            var ea = await _db.Set<EntityAddress>()
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == entityAddressId && x.EntityType == EntityType.PERSON && x.EntityId == personId.Value, ct);

            if (ea is null || ea.Address is null) return false;

            var now = DateTimeOffset.UtcNow;

            if (req.IsPrimary)
            {
                var primaries = await _db.Set<EntityAddress>()
                    .Where(x => x.EntityType == EntityType.PERSON && x.EntityId == personId.Value && x.AddressType == req.AddressType && x.IsPrimary && x.Id != ea.Id)
                    .ToListAsync(ct);

                foreach (var p in primaries) p.IsPrimary = false;
            }

            ea.AddressType = req.AddressType;
            ea.IsPrimary = req.IsPrimary;

            ea.Address.Country = req.Country;
            ea.Address.PostalCode = req.PostalCode;
            ea.Address.City = req.City;
            ea.Address.Street = req.Street;
            ea.Address.BuildingNumber = req.BuildingNumber;
            ea.Address.ApartmentNumber = req.ApartmentNumber;
            ea.Address.FullText = req.FullText;
            ea.Address.UpdatedAt = now;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAddressAsync(long employeeId, long entityAddressId, long managerUserId, CancellationToken ct)
        {
            var personId = await _db.Set<Employee>()
                .AsNoTracking()
                .Where(e => e.Id == employeeId)
                .Select(e => (long?)e.PersonId)
                .FirstOrDefaultAsync(ct);

            if (personId is null) return false;

            var ea = await _db.Set<EntityAddress>()
                .FirstOrDefaultAsync(x => x.Id == entityAddressId && x.EntityType == EntityType.PERSON && x.EntityId == personId.Value, ct);

            if (ea is null) return false;

            _db.Remove(ea);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // BANK ACCOUNTS
        public async Task<IReadOnlyList<EmployeeBankAccountDto>> GetBankAccountsAsync(long employeeId, CancellationToken ct)
        {
            return await _db.Set<EmployeeBankAccount>()
                .AsNoTracking()
                .Where(b => b.EmployeeId == employeeId)
                .OrderByDescending(b => b.IsPrimary)
                .ThenByDescending(b => b.UpdatedAt)
                .Select(b => new EmployeeBankAccountDto
                {
                    Id = b.Id,
                    IsPrimary = b.IsPrimary,
                    Country = b.Country,
                    AccountLast4 = b.AccountLast4,
                    ValidFrom = b.ValidFrom,
                    ValidTo = b.ValidTo,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt
                })
                .ToListAsync(ct);
        }

        public async Task<long> AddBankAccountAsync(long employeeId, EmployeeBankAccountUpsertRequest req, long managerUserId, CancellationToken ct)
        {
            var exists = await _db.Set<Employee>().AnyAsync(e => e.Id == employeeId, ct);
            if (!exists) throw new InvalidOperationException("Employee not found.");

            var now = DateTimeOffset.UtcNow;

            if (req.IsPrimary)
            {
                var primaries = await _db.Set<EmployeeBankAccount>()
                    .Where(x => x.EmployeeId == employeeId && x.IsPrimary)
                    .ToListAsync(ct);

                foreach (var p in primaries) p.IsPrimary = false;
            }

            var encrypted = _protector.Encrypt(req.AccountPlain);
            var last4 = req.AccountPlain.Length >= 4 ? req.AccountPlain[^4..] : req.AccountPlain;

            var ba = new EmployeeBankAccount
            {
                EmployeeId = employeeId,
                AccountEncrypted = encrypted,
                AccountLast4 = last4,
                Country = req.Country,
                IsPrimary = req.IsPrimary,
                ValidFrom = req.ValidFrom,
                ValidTo = req.ValidTo,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Add(ba);
            await _db.SaveChangesAsync(ct);
            return ba.Id;
        }

        public async Task<bool> UpdateBankAccountAsync(long employeeId, long bankAccountId, EmployeeBankAccountUpsertRequest req, long managerUserId, CancellationToken ct)
        {
            var ba = await _db.Set<EmployeeBankAccount>()
                .FirstOrDefaultAsync(x => x.Id == bankAccountId && x.EmployeeId == employeeId, ct);

            if (ba is null) return false;

            if (req.IsPrimary)
            {
                var primaries = await _db.Set<EmployeeBankAccount>()
                    .Where(x => x.EmployeeId == employeeId && x.IsPrimary && x.Id != ba.Id)
                    .ToListAsync(ct);

                foreach (var p in primaries) p.IsPrimary = false;
            }

            ba.Country = req.Country;
            ba.IsPrimary = req.IsPrimary;
            ba.ValidFrom = req.ValidFrom;
            ba.ValidTo = req.ValidTo;
            ba.AccountEncrypted = _protector.Encrypt(req.AccountPlain);
            ba.AccountLast4 = req.AccountPlain.Length >= 4 ? req.AccountPlain[^4..] : req.AccountPlain;
            ba.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteBankAccountAsync(long employeeId, long bankAccountId, long managerUserId, CancellationToken ct)
        {
            var ba = await _db.Set<EmployeeBankAccount>()
                .FirstOrDefaultAsync(x => x.Id == bankAccountId && x.EmployeeId == employeeId, ct);

            if (ba is null) return false;

            _db.Remove(ba);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // HELPERS
        private Task<CurrentProjectDto?> GetCurrentProjectAsync(long employeeId, CancellationToken ct)
            => _db.Set<WorkSession>()
                .AsNoTracking()
                .Where(ws => ws.EmployeeId == employeeId && ws.EndedAt == null)
                .OrderByDescending(ws => ws.StartedAt)
                .Select(ws => new CurrentProjectDto
                {
                    WorkSessionId = ws.Id,
                    ProjectId = ws.ProjectId,
                    Code = ws.Project!.Code,
                    Name = ws.Project!.Name,
                    StartedAt = ws.StartedAt,
                    Status = ws.Status
                })
                .FirstOrDefaultAsync(ct);

        private Task<CurrentVehicleDto?> GetCurrentVehicleAsync(long employeeId, CancellationToken ct)
            => _db.Set<VehicleAssignment>()
                .AsNoTracking()
                .Where(a => a.EmployeeId == employeeId && a.ReturnedAt == null)
                .OrderByDescending(a => a.AssignedAt)
                .Select(a => new CurrentVehicleDto
                {
                    VehicleAssignmentId = a.Id,
                    VehicleId = a.VehicleId,
                    Name = a.Vehicle!.Name,
                    PlateNumber = a.Vehicle!.PlateNumber,
                    AssignedAt = a.AssignedAt
                })
                .FirstOrDefaultAsync(ct);

        private Task<List<EmployeeToolInDetailsDto>> GetCurrentEmployeeToolsAsync(long employeeId, CancellationToken ct)
            => _db.Set<ToolAssignment>()
                .AsNoTracking()
                .Where(a => a.ToEmployeeId == employeeId && a.ReturnedAt == null)
                .OrderByDescending(a => a.AssignedAt)
                .Select(a => new EmployeeToolInDetailsDto
                {
                    ToolAssignmentId = a.Id,
                    ToolAssetId = a.ToolAssetId,
                    ToolId = a.ToolAsset!.ToolId,
                    ToolName = a.ToolAsset!.Tool!.Name,
                    ToolSku = a.ToolAsset!.Tool!.Sku,
                    SerialNumber = a.ToolAsset!.SerialNumber,
                    AssetTag = a.ToolAsset!.AssetTag,
                    Condition = a.ToolAsset!.Condition,
                    ToolAssetStatus = a.ToolAsset!.Status,
                    AssignedAt = a.AssignedAt,
                    Notes = a.Notes
                })
                .ToListAsync(ct);

        private Task<List<EmployeeAddressDto>> GetAddressesInternalAsync(long personId, CancellationToken ct)
            => _db.Set<EntityAddress>()
                .AsNoTracking()
                .Where(ea => ea.EntityType == EntityType.PERSON && ea.EntityId == personId)
                .OrderByDescending(ea => ea.IsPrimary)
                .ThenBy(ea => ea.AddressType)
                .Select(ea => new EmployeeAddressDto
                {
                    EntityAddressId = ea.Id,
                    AddressType = ea.AddressType,
                    IsPrimary = ea.IsPrimary,
                    AddressId = ea.AddressId,
                    Country = ea.Address!.Country,
                    PostalCode = ea.Address!.PostalCode,
                    City = ea.Address!.City,
                    Street = ea.Address!.Street,
                    BuildingNumber = ea.Address!.BuildingNumber,
                    ApartmentNumber = ea.Address!.ApartmentNumber,
                    FullText = ea.Address!.FullText
                })
                .ToListAsync(ct);

        public async Task<IReadOnlyList<EmployeeToolInDetailsDto>?> GetToolsAsync(long employeeId, CancellationToken ct)
        {
            var exists = await _db.Set<Employee>()
                .AsNoTracking()
                .AnyAsync(e => e.Id == employeeId, ct);

            if (!exists) return null;

            return await _db.Set<ToolAssignment>()
                .AsNoTracking()
                .Where(a => a.ToEmployeeId == employeeId && a.ReturnedAt == null)
                .OrderByDescending(a => a.AssignedAt)
                .Select(a => new EmployeeToolInDetailsDto
                {
                    ToolAssignmentId = a.Id,
                    ToolAssetId = a.ToolAssetId,
                    ToolId = a.ToolAsset!.ToolId,
                    ToolName = a.ToolAsset!.Tool!.Name,
                    ToolSku = a.ToolAsset!.Tool!.Sku,
                    SerialNumber = a.ToolAsset!.SerialNumber,
                    AssetTag = a.ToolAsset!.AssetTag,
                    Condition = a.ToolAsset!.Condition,
                    ToolAssetStatus = a.ToolAsset!.Status,
                    AssignedAt = a.AssignedAt,
                    Notes = a.Notes
                })
                .ToListAsync(ct);
        }
        public async Task<bool> ReturnMyToolAsync(long toolAssignmentId, long userId, CancellationToken ct)
        {
            var employeeId = await GetEmployeeIdByUserIdAsync(userId, ct);
            if (employeeId is null) return false;

            var a = await _db.Set<ToolAssignment>()
                .FirstOrDefaultAsync(x =>
                    x.Id == toolAssignmentId &&
                    x.ToEmployeeId == employeeId.Value &&
                    x.ReturnedAt == null, ct);

            if (a is null) return false;

            a.ReturnedAt = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        private async Task<long?> GetEmployeeIdByUserIdAsync(long userId, CancellationToken ct)
        {
            return await _db.Set<Employee>()
                .AsNoTracking()
                .Where(e => e.Person!.UserId == userId)
                .Select(e => (long?)e.Id)
                .FirstOrDefaultAsync(ct);
        }
        public async Task<Employee?> GetByIdAsync(long id, CancellationToken ct)
        {
            return await _db.Set<Employee>()
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == id, ct);
        }
    }
}
