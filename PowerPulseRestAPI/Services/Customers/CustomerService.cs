using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.AddressModels;
using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.DTO.CustomerDto.Request;
using PowerPulseRestAPI.DTO.CustomerDto.Response;
using Microsoft.EntityFrameworkCore;

namespace PowerPulseRestAPI.Services.Customers
{
    public sealed class CustomerService : ICustomerService
    {
        private readonly PowerPulseContext _db;

        public CustomerService(PowerPulseContext db)
        {
            _db = db;
        }

        // WORKER
        public async Task<IReadOnlyList<CustomerListItemDto>> GetListAsync(
            string? q,
            CustomerStatus? status,
            int skip,
            int take,
            CancellationToken ct)
        {
            var query = _db.Set<Customer>().AsNoTracking();

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var s = q.Trim();
                query = query.Where(c =>
                    c.Name.Contains(s) ||
                    (c.TaxId != null && c.TaxId.Contains(s)) ||
                    (c.Email != null && c.Email.Contains(s)) ||
                    (c.Phone != null && c.Phone.Contains(s)));
            }

            return await query
                .OrderBy(c => c.Name)
                .Skip(skip)
                .Take(take)
                .Select(c => new CustomerListItemDto
                {
                    Id = c.Id,
                    CustomerType = c.CustomerType,
                    Status = c.Status,
                    Name = c.Name,
                    TaxId = c.TaxId,
                    Email = c.Email,
                    Phone = c.Phone,

                    PrimaryContact = c.Contacts
                        .OrderByDescending(x => x.IsPrimary)
                        .ThenByDescending(x => x.UpdatedAt)
                        .Select(x => new CustomerPrimaryContactDto
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Email = x.Email,
                            Phone = x.Phone,
                            RoleTitle = x.RoleTitle
                        })
                        .FirstOrDefault(),

                    ProjectsCount = c.ProjectCustomers.Count,
                    ActiveProjectsCount = c.ProjectCustomers.Count(pc => pc.Project!.Status == ProjectStatus.ACTIVE),
                    CurrentProject = c.ProjectCustomers
                        .Where(pc => pc.Project!.Status == ProjectStatus.ACTIVE)
                        .OrderByDescending(pc => pc.Project!.UpdatedAt)
                        .Select(pc => new ProjectInCustomerDto
                        {
                            ProjectId = pc.ProjectId,
                            Code = pc.Project!.Code,
                            Name = pc.Project!.Name,
                            Status = pc.Project!.Status,
                            RelationshipType = pc.RelationshipType,
                            IsPrimaryOwner = pc.IsPrimaryOwner
                        })
                        .FirstOrDefault()
                })
                .ToListAsync(ct);
        }

        public Task<CustomerPublicDetailsDto?> GetPublicDetailsAsync(long customerId, CancellationToken ct)
            => _db.Set<Customer>()
                .AsNoTracking()
                .Where(c => c.Id == customerId)
                .Select(c => new CustomerPublicDetailsDto
                {
                    Id = c.Id,
                    CustomerType = c.CustomerType,
                    Status = c.Status,
                    Name = c.Name,
                    TaxId = c.TaxId,
                    Email = c.Email,
                    Phone = c.Phone,

                    PrimaryContact = c.Contacts
                        .OrderByDescending(x => x.IsPrimary)
                        .ThenByDescending(x => x.UpdatedAt)
                        .Select(x => new CustomerPrimaryContactDto
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Email = x.Email,
                            Phone = x.Phone,
                            RoleTitle = x.RoleTitle
                        })
                        .FirstOrDefault(),

                    Projects = c.ProjectCustomers
                        .OrderByDescending(pc => pc.CreatedAt)
                        .Select(pc => new ProjectInCustomerDto
                        {
                            ProjectId = pc.ProjectId,
                            Code = pc.Project!.Code,
                            Name = pc.Project!.Name,
                            Status = pc.Project!.Status,
                            RelationshipType = pc.RelationshipType,
                            IsPrimaryOwner = pc.IsPrimaryOwner
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);

        // MANAGER
        public async Task<CustomerDetailsPrivateDto?> GetManagerDetailsAsync(long customerId, CancellationToken ct)
        {
            var dto = await _db.Set<Customer>()
                .AsNoTracking()
                .Where(c => c.Id == customerId)
                .Select(c => new CustomerDetailsPrivateDto
                {
                    Id = c.Id,
                    CustomerType = c.CustomerType,
                    Status = c.Status,
                    Name = c.Name,
                    TaxId = c.TaxId,
                    Email = c.Email,
                    Phone = c.Phone,
                    Note = c.Note,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,

                    Contacts = c.Contacts
                        .OrderByDescending(x => x.IsPrimary)
                        .ThenBy(x => x.LastName)
                        .Select(x => new CustomerContactDto
                        {
                            Id = x.Id,
                            CustomerId = x.CustomerId,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            RoleTitle = x.RoleTitle,
                            Email = x.Email,
                            Phone = x.Phone,
                            IsPrimary = x.IsPrimary,
                            Note = x.Note,
                            CreatedAt = x.CreatedAt,
                            UpdatedAt = x.UpdatedAt
                        })
                        .ToList(),

                    Projects = c.ProjectCustomers
                        .OrderByDescending(pc => pc.CreatedAt)
                        .Select(pc => new ProjectInCustomerDto
                        {
                            ProjectId = pc.ProjectId,
                            Code = pc.Project!.Code,
                            Name = pc.Project!.Name,
                            Status = pc.Project!.Status,
                            RelationshipType = pc.RelationshipType,
                            IsPrimaryOwner = pc.IsPrimaryOwner
                        })
                        .ToList(),

                    Notes = c.Notes
                        .OrderByDescending(n => n.CreatedAt)
                        .Select(n => new CustomerNoteDto
                        {
                            Id = n.Id,
                            NoteType = n.NoteType,
                            Content = n.Content,
                            CreatedByUserId = n.CreatedByUserId,
                            CreatedAt = n.CreatedAt
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (dto is null) return null;

            dto.Addresses = await _db.Set<EntityAddress>()
                .AsNoTracking()
                .Where(ea => ea.EntityType == EntityType.CUSTOMER && ea.EntityId == customerId)
                .OrderByDescending(ea => ea.IsPrimary)
                .ThenBy(ea => ea.AddressType)
                .Select(ea => new CustomerAddressDto
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
                    FullText = ea.Address!.FullText,
                })
                .ToListAsync(ct);

            return dto;
        }

        public async Task<long> CreateAsync(CustomerCreateRequest req, long managerUserId, CancellationToken ct)
        {
            var now = DateTimeOffset.UtcNow;

            await using var tx = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                if (!string.IsNullOrWhiteSpace(req.TaxId))
                {
                    var dup = await _db.Set<Customer>()
                        .AsNoTracking()
                        .AnyAsync(c => c.TaxId == req.TaxId, ct);

                    if (dup) throw new InvalidOperationException("Customer with given TaxId already exists.");
                }

                var customer = new Customer
                {
                    CustomerType = req.CustomerType,
                    Status = req.Status,
                    Name = req.Name,
                    TaxId = req.TaxId,
                    Email = req.Email,
                    Phone = req.Phone,
                    Note = req.Note,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                _db.Add(customer);
                await _db.SaveChangesAsync(ct);

                // optional primary contact
                if (req.PrimaryContact is not null)
                {
                    _db.Add(new CustomerContact
                    {
                        CustomerId = customer.Id,
                        FirstName = req.PrimaryContact.FirstName,
                        LastName = req.PrimaryContact.LastName,
                        RoleTitle = req.PrimaryContact.RoleTitle,
                        Email = req.PrimaryContact.Email,
                        Phone = req.PrimaryContact.Phone,
                        IsPrimary = true,
                        Note = req.PrimaryContact.Note,
                        CreatedAt = now,
                        UpdatedAt = now
                    });
                }

                // optional main address
                if (req.MainAddress is not null)
                {
                    var addr = new Address
                    {
                        Country = req.MainAddress.Country,
                        PostalCode = req.MainAddress.PostalCode,
                        City = req.MainAddress.City,
                        Street = req.MainAddress.Street,
                        BuildingNumber = req.MainAddress.BuildingNumber,
                        ApartmentNumber = req.MainAddress.ApartmentNumber,
                        FullText = req.MainAddress.FullText,
                        Latitude = req.MainAddress.Latitude,
                        Longitude = req.MainAddress.Longitude,
                        CreatedAt = now,
                        UpdatedAt = now
                    };

                    _db.Add(new EntityAddress
                    {
                        Address = addr,
                        EntityType = EntityType.CUSTOMER,
                        EntityId = customer.Id,
                        AddressType = req.MainAddress.AddressType,
                        IsPrimary = true,
                        CreatedAt = now
                    });
                }

                await _db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);

                return customer.Id;
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long customerId, CustomerUpdateRequest req, long managerUserId, CancellationToken ct)
        {
            var c = await _db.Set<Customer>().FirstOrDefaultAsync(x => x.Id == customerId, ct);
            if (c is null) return false;

            if (!string.IsNullOrWhiteSpace(req.TaxId) && req.TaxId != c.TaxId)
            {
                var dup = await _db.Set<Customer>()
                    .AsNoTracking()
                    .AnyAsync(x => x.TaxId == req.TaxId && x.Id != customerId, ct);

                if (dup) throw new InvalidOperationException("Customer with given TaxId already exists.");
            }

            c.CustomerType = req.CustomerType;
            c.Status = req.Status;
            c.Name = req.Name;
            c.TaxId = req.TaxId;
            c.Email = req.Email;
            c.Phone = req.Phone;
            c.Note = req.Note;
            c.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(long customerId, long managerUserId, CancellationToken ct)
        {
            var c = await _db.Set<Customer>().FirstOrDefaultAsync(x => x.Id == customerId, ct);
            if (c is null) return false;

            var hasInvoices = await _db.Set<Invoice>().AnyAsync(i => i.CustomerId == customerId, ct);
            if (hasInvoices) throw new InvalidOperationException("Cannot delete customer with invoices.");

            c.Status = CustomerStatus.INACTIVE;
            c.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        // CONTACTS
        public async Task<long> AddContactAsync(long customerId, CustomerContactUpsertRequest req, long managerUserId, CancellationToken ct)
        {
            var exists = await _db.Set<Customer>().AnyAsync(c => c.Id == customerId, ct);
            if (!exists) throw new InvalidOperationException("Customer not found.");

            var now = DateTimeOffset.UtcNow;
            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            if (req.IsPrimary)
            {
                await _db.Set<CustomerContact>()
                    .Where(x => x.CustomerId == customerId && x.IsPrimary)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsPrimary, false), ct);
            }

            var cc = new CustomerContact
            {
                CustomerId = customerId,
                FirstName = req.FirstName,
                LastName = req.LastName,
                RoleTitle = req.RoleTitle,
                Email = req.Email,
                Phone = req.Phone,
                IsPrimary = req.IsPrimary,
                Note = req.Note,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Add(cc);
            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return cc.Id;
        }

        public async Task<bool> UpdateContactAsync(long customerId, long contactId, CustomerContactUpsertRequest req, long managerUserId, CancellationToken ct)
        {
            var cc = await _db.Set<CustomerContact>()
                .FirstOrDefaultAsync(x => x.Id == contactId && x.CustomerId == customerId, ct);

            if (cc is null) return false;

            var now = DateTimeOffset.UtcNow;
            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            if (req.IsPrimary)
            {
                await _db.Set<CustomerContact>()
                    .Where(x => x.CustomerId == customerId && x.IsPrimary && x.Id != contactId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsPrimary, false), ct);
            }

            cc.FirstName = req.FirstName;
            cc.LastName = req.LastName;
            cc.RoleTitle = req.RoleTitle;
            cc.Email = req.Email;
            cc.Phone = req.Phone;
            cc.IsPrimary = req.IsPrimary;
            cc.Note = req.Note;
            cc.UpdatedAt = now;

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return true;
        }

        public async Task<bool> DeleteContactAsync(long customerId, long contactId, long managerUserId, CancellationToken ct)
        {
            var cc = await _db.Set<CustomerContact>()
                .FirstOrDefaultAsync(x => x.Id == contactId && x.CustomerId == customerId, ct);

            if (cc is null) return false;

            var isLinkedToProjects = await _db.Set<ProjectCustomerContact>()
                .AnyAsync(x => x.CustomerContactId == contactId, ct);

            if (isLinkedToProjects) throw new InvalidOperationException("Cannot delete contact linked to projects.");

            _db.Remove(cc);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // ADDRESSES
        public async Task<long> AddAddressAsync(long customerId, CustomerAddressUpsertRequest req, long managerUserId, CancellationToken ct)
        {
            var exists = await _db.Set<Customer>().AnyAsync(c => c.Id == customerId, ct);
            if (!exists) throw new InvalidOperationException("Customer not found.");

            var now = DateTimeOffset.UtcNow;
            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            if (req.IsPrimary)
            {
                await _db.Set<EntityAddress>()
                    .Where(ea =>
                        ea.EntityType == EntityType.CUSTOMER &&
                        ea.EntityId == customerId &&
                        ea.AddressType == req.AddressType &&
                        ea.IsPrimary)
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsPrimary, false), ct);
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
                Latitude = req.Latitude,
                Longitude = req.Longitude,
                CreatedAt = now,
                UpdatedAt = now
            };

            var link = new EntityAddress
            {
                Address = addr,
                EntityType = EntityType.CUSTOMER,
                EntityId = customerId,
                AddressType = req.AddressType,
                IsPrimary = req.IsPrimary,
                CreatedAt = now
            };

            _db.Add(link);
            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return link.Id;
        }

        public async Task<bool> UpdateAddressAsync(long customerId, long entityAddressId, CustomerAddressUpsertRequest req, long managerUserId, CancellationToken ct)
        {
            var ea = await _db.Set<EntityAddress>()
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x =>
                    x.Id == entityAddressId &&
                    x.EntityType == EntityType.CUSTOMER &&
                    x.EntityId == customerId, ct);

            if (ea is null || ea.Address is null) return false;

            var now = DateTimeOffset.UtcNow;
            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            if (req.IsPrimary)
            {
                await _db.Set<EntityAddress>()
                    .Where(x =>
                        x.EntityType == EntityType.CUSTOMER &&
                        x.EntityId == customerId &&
                        x.AddressType == req.AddressType &&
                        x.IsPrimary &&
                        x.Id != entityAddressId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsPrimary, false), ct);
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
            ea.Address.Latitude = req.Latitude;
            ea.Address.Longitude = req.Longitude;
            ea.Address.UpdatedAt = now;

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return true;
        }

        public async Task<bool> DeleteAddressAsync(long customerId, long entityAddressId, long managerUserId, CancellationToken ct)
        {
            var ea = await _db.Set<EntityAddress>()
                .FirstOrDefaultAsync(x =>
                    x.Id == entityAddressId &&
                    x.EntityType == EntityType.CUSTOMER &&
                    x.EntityId == customerId, ct);

            if (ea is null) return false;

            _db.Remove(ea);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // NOTES
        public async Task<long> AddNoteAsync(long customerId, CustomerNoteCreateRequest req, long managerUserId, CancellationToken ct)
        {
            var exists = await _db.Set<Customer>().AnyAsync(c => c.Id == customerId, ct);
            if (!exists) throw new InvalidOperationException("Customer not found.");

            var note = new CustomerNote
            {
                CustomerId = customerId,
                NoteType = req.NoteType,
                Content = req.Content,
                CreatedByUserId = managerUserId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _db.Add(note);
            await _db.SaveChangesAsync(ct);
            return note.Id;
        }

        // INVOICES
        public async Task<IReadOnlyList<CustomerInvoiceListItemDto>> GetInvoicesAsync(
            long customerId,
            InvoiceStatus? status,
            DateOnly? from,
            DateOnly? to,
            int skip,
            int take,
            CancellationToken ct)
        {
            var q = _db.Set<Invoice>()
                .AsNoTracking()
                .Where(i => i.CustomerId == customerId);

            if (status.HasValue) q = q.Where(i => i.Status == status.Value);
            if (from.HasValue) q = q.Where(i => i.IssueDate >= from.Value);
            if (to.HasValue) q = q.Where(i => i.IssueDate <= to.Value);

            return await q
                .OrderByDescending(i => i.IssueDate)
                .Skip(skip)
                .Take(take)
                .Select(i => new CustomerInvoiceListItemDto
                {
                    Id = i.Id,
                    InvoiceNumber = i.InvoiceNumber,
                    Status = i.Status,
                    IssueDate = i.IssueDate,
                    DueDate = i.DueDate,
                    Currency = i.Currency,
                    TotalAmount = i.TotalAmount,
                    ProjectId = i.ProjectId,
                    ProjectCode = i.Project != null ? i.Project.Code : null,
                    ProjectName = i.Project != null ? i.Project.Name : null
                })
                .ToListAsync(ct);
        }
    }
}
