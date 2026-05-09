using PowerPulseRestAPI.Data.Models.AddressModels;
using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.PersonModels;
using PowerPulseRestAPI.DTO.CustomerDto.Requests;
using PowerPulseRestAPI.DTO.CustomerDto.Responses;
using PowerPulseRestAPI.Mappers.Customers;
using PowerPulseRestAPI.Services.Uploads;

namespace PowerPulseRestAPI.Services.Customers
{
    public sealed class CustomerService : ICustomerService
    {
        private readonly PowerPulseContext _dbContext;
        private readonly IFileStorageService _fileStorageService;

        public CustomerService(PowerPulseContext db, IFileStorageService fileStorageService)
        {
            _dbContext = db;
            _fileStorageService = fileStorageService;
        }

        public async Task<CustomerDetailsDto> CreateAsync(
            CreateCustomerDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var taxIdExists = await _dbContext.Customers
                .IgnoreQueryFilters()
                .AnyAsync(x => x.TaxId == dto.TaxId, cancellationToken);

            if (taxIdExists)
            {
                throw new InvalidOperationException($"Customer with TaxId '{dto.TaxId}' already exists.");
            }

            var personEmailExists = await _dbContext.Persons
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Email == dto.ContactPerson.Email, cancellationToken);

            if (personEmailExists)
            {
                throw new InvalidOperationException($"Person with email '{dto.ContactPerson.Email}' already exists.");
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var customerAddress = new Address
            {
                Country = dto.Address.Country,
                PostalCode = dto.Address.PostalCode,
                City = dto.Address.City,
                Street = dto.Address.Street,
                BuildingNumber = dto.Address.BuildingNumber,
                ApartmentNumber = dto.Address.ApartmentNumber,
                FullText = dto.Address.FullText,
                AddressType = dto.Address.AddressType,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            _dbContext.Addresses.Add(customerAddress);
            await _dbContext.SaveChangesAsync(cancellationToken);

            Address? contactPersonAddress = null;
            if (dto.ContactPerson.Address is not null)
            {
                contactPersonAddress = new Address
                {
                    Country = dto.ContactPerson.Address.Country,
                    PostalCode = dto.ContactPerson.Address.PostalCode,
                    City = dto.ContactPerson.Address.City,
                    Street = dto.ContactPerson.Address.Street,
                    BuildingNumber = dto.ContactPerson.Address.BuildingNumber,
                    ApartmentNumber = dto.ContactPerson.Address.ApartmentNumber,
                    FullText = dto.ContactPerson.Address.FullText,
                    AddressType = dto.ContactPerson.Address.AddressType,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };

                _dbContext.Addresses.Add(contactPersonAddress);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            var contactPerson = new Person
            {
                FirstName = dto.ContactPerson.FirstName,
                LastName = dto.ContactPerson.LastName,
                Phone = dto.ContactPerson.Phone,
                DateOfBirth = dto.ContactPerson.DateOfBirth ?? default,
                AvatarUrl = dto.ContactPerson.AvatarUrl,
                Email = dto.ContactPerson.Email,
                AddressId = contactPersonAddress?.Id,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            _dbContext.Persons.Add(contactPerson);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var customer = new Customer
            {
                Status = dto.Status,
                CompanyName = dto.CompanyName,
                PhoneNumber = dto.PhoneNumber,
                TaxId = dto.TaxId,
                AvatarUrl = dto.AvatarUrl,
                ContactPersonId = contactPerson.Id,
                AddressId = customerAddress.Id,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                IsDeleted = false
            };

            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            var created = await GetCustomerQuery()
                .FirstAsync(x => x.Id == customer.Id, cancellationToken);

            return created.ToDetailsDto();
        }

        public async Task<IReadOnlyList<CustomerListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default)
        {
            var customers = await GetCustomerQuery()
                .OrderBy(x => x.CompanyName)
                .ToListAsync(cancellationToken);

            return customers
                .Select(x => x.ToListItemDto())
                .ToList();
        }

        public async Task<CustomerDetailsDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var customer = await GetCustomerQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (customer is null)
            {
                throw new KeyNotFoundException($"Customer with id '{id}' was not found.");
            }

            return customer.ToDetailsDto();
        }

        public async Task<CustomerDetailsDto> UpdateAsync(
            long id,
            UpdateCustomerDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var customer = await _dbContext.Customers
                .Include(x => x.Address)
                .Include(x => x.ContactPerson)
                    .ThenInclude(x => x.Address)
                .Include(x => x.Projects)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);


            if (customer is null)
            {
                throw new KeyNotFoundException($"Customer with id '{id}' was not found.");
            }

            var taxIdExists = await _dbContext.Customers
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Id != id && x.TaxId == dto.TaxId, cancellationToken);

            if (taxIdExists)
            {
                throw new InvalidOperationException($"Customer with TaxId '{dto.TaxId}' already exists.");
            }

            var personEmailExists = await _dbContext.Persons
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Id != customer.ContactPersonId && x.Email == dto.ContactPerson.Email, cancellationToken);

            if (personEmailExists)
            {
                throw new InvalidOperationException($"Person with email '{dto.ContactPerson.Email}' already exists.");
            }

            var oldCustomerAvatarUrl = customer.AvatarUrl;
            var oldContactPersonAvatarUrl = customer.ContactPerson.AvatarUrl;

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            customer.Status = dto.Status;
            customer.CompanyName = dto.CompanyName;
            customer.PhoneNumber = dto.PhoneNumber;
            customer.TaxId = dto.TaxId;
            customer.AvatarUrl = dto.AvatarUrl;
            customer.UpdatedAt = DateTimeOffset.UtcNow;

            customer.Address.Country = dto.Address.Country;
            customer.Address.PostalCode = dto.Address.PostalCode;
            customer.Address.City = dto.Address.City;
            customer.Address.Street = dto.Address.Street;
            customer.Address.BuildingNumber = dto.Address.BuildingNumber;
            customer.Address.ApartmentNumber = dto.Address.ApartmentNumber;
            customer.Address.FullText = dto.Address.FullText;
            customer.Address.AddressType = dto.Address.AddressType;
            customer.Address.UpdatedAt = DateTimeOffset.UtcNow;

            customer.ContactPerson.FirstName = dto.ContactPerson.FirstName!;
            customer.ContactPerson.LastName = dto.ContactPerson.LastName!;
            customer.ContactPerson.Phone = dto.ContactPerson.Phone!;
            customer.ContactPerson.DateOfBirth = dto.ContactPerson.DateOfBirth ?? default;
            customer.ContactPerson.AvatarUrl = dto.ContactPerson.AvatarUrl;
            customer.ContactPerson.Email = dto.ContactPerson.Email!;
            customer.ContactPerson.UpdatedAt = DateTimeOffset.UtcNow;

            if (dto.ContactPerson.Address is null)
            {
                if (customer.ContactPerson.Address is not null)
                {
                    customer.ContactPerson.Address = null;
                    customer.ContactPerson.AddressId = null;
                }
            }
            else
            {
                if (customer.ContactPerson.Address is null)
                {
                    customer.ContactPerson.Address = new Address
                    {
                        Country = dto.ContactPerson.Address.Country,
                        PostalCode = dto.ContactPerson.Address.PostalCode,
                        City = dto.ContactPerson.Address.City,
                        Street = dto.ContactPerson.Address.Street,
                        BuildingNumber = dto.ContactPerson.Address.BuildingNumber,
                        ApartmentNumber = dto.ContactPerson.Address.ApartmentNumber,
                        FullText = dto.ContactPerson.Address.FullText,
                        AddressType = dto.ContactPerson.Address.AddressType,
                        CreatedAt = DateTimeOffset.UtcNow,
                        UpdatedAt = DateTimeOffset.UtcNow
                    };
                }
                else
                {
                    customer.ContactPerson.Address.Country = dto.ContactPerson.Address.Country;
                    customer.ContactPerson.Address.PostalCode = dto.ContactPerson.Address.PostalCode;
                    customer.ContactPerson.Address.City = dto.ContactPerson.Address.City;
                    customer.ContactPerson.Address.Street = dto.ContactPerson.Address.Street;
                    customer.ContactPerson.Address.BuildingNumber = dto.ContactPerson.Address.BuildingNumber;
                    customer.ContactPerson.Address.ApartmentNumber = dto.ContactPerson.Address.ApartmentNumber;
                    customer.ContactPerson.Address.FullText = dto.ContactPerson.Address.FullText;
                    customer.ContactPerson.Address.AddressType = dto.ContactPerson.Address.AddressType;
                    customer.ContactPerson.Address.UpdatedAt = DateTimeOffset.UtcNow;
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            if (!string.Equals(oldCustomerAvatarUrl, customer.AvatarUrl, StringComparison.OrdinalIgnoreCase))
{
            _fileStorageService.DeleteFileByUrl(oldCustomerAvatarUrl);
}

            if (!string.Equals(oldContactPersonAvatarUrl, customer.ContactPerson.AvatarUrl, StringComparison.OrdinalIgnoreCase))
            {
                _fileStorageService.DeleteFileByUrl(oldContactPersonAvatarUrl);
            }

            var updated = await GetCustomerQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailsDto();
        }

        public async Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (customer is null)
            {
                throw new KeyNotFoundException($"Customer with id '{id}' was not found.");
            }

            customer.IsDeleted = true;
            customer.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<Customer> GetCustomerQuery()
        {
            return _dbContext.Customers
                .AsNoTracking()
                .Include(x => x.Address)
                .Include(x => x.ContactPerson)
                    .ThenInclude(x => x.Address)
                .Include(x => x.Projects);
        }

    }
}
