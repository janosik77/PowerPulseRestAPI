using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.DTO.InvoiceDto.Requests;
using PowerPulseRestAPI.DTO.InvoiceDto.Responses;
using PowerPulseRestAPI.Mappers.Invoices;

namespace PowerPulseRestAPI.Services.InvoiceS
{
    public class InvoiceService : IInvoiceService
    {
        private readonly PowerPulseContext _dbContext;

        public InvoiceService(PowerPulseContext db)
        {
            _dbContext = db;
        }

        public async Task<InvoiceDetailsDto> CreateAsync(
            CreateInvoiceDto dto,
            long createdByUserId,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.DueDate < dto.IssueDate)
            {
                throw new InvalidOperationException("DueDate cannot be earlier than IssueDate.");
            }

            if (dto.BillingPeriodEnd < dto.BillingPeriodStart)
            {
                throw new InvalidOperationException("Billing period end cannot be earlier than billing period start.");
            }

            var project = await _dbContext.Projects
                .Include(x => x.Customer)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == dto.ProjectId, cancellationToken);

            var allMaterials = await _dbContext.Materials
                .AsNoTracking()
                .ToDictionaryAsync(x => x.Id, cancellationToken);

            if (project is null)
            {
                throw new KeyNotFoundException($"Project with id '{dto.ProjectId}' was not found.");
            }

            var userExists = await _dbContext.Users
                .AnyAsync(x => x.Id == createdByUserId, cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException($"User with id '{createdByUserId}' was not found.");
            }

            var laborInput = dto.Items.FirstOrDefault(x => x.ItemType == InvoiceFormItemType.LABOR);

            var materialInputs = dto.Items
                .Where(x => x.ItemType == InvoiceFormItemType.MATERIAL)
                .ToList();

            var workSessions = await _dbContext.WorkSessions
                .Where(x =>
                    x.ProjectId == dto.ProjectId &&
                    x.InvoiceId == null &&
                    x.EndedAt != null &&
                    x.StartedAt.Date >= dto.BillingPeriodStart.ToDateTime(TimeOnly.MinValue) &&
                    x.StartedAt.Date <= dto.BillingPeriodEnd.ToDateTime(TimeOnly.MaxValue))
                .ToListAsync(cancellationToken);

            var periodStart = dto.BillingPeriodStart.ToDateTime(TimeOnly.MinValue);
            var periodEnd = dto.BillingPeriodEnd.ToDateTime(TimeOnly.MaxValue);

            var materialMovements = await _dbContext.MaterialMovements
                .Include(x => x.Material)
                .Where(x =>
                    x.ProjectId == dto.ProjectId &&
                    x.InvoiceId == null &&
                    x.OccurredAt >= periodStart &&
                    x.OccurredAt <= periodEnd &&
                    (x.MovementType == MaterialMovementType.PROJECT_CONSUME))
                .ToListAsync(cancellationToken);

            if (laborInput is not null && workSessions.Count == 0)
            {
                throw new InvalidOperationException("No uninvoiced work sessions found in selected billing period.");
            }

            foreach (var materialInput in materialInputs)
            {
                var hasMatchingMovement = materialMovements.Any(x => x.MaterialId == materialInput.MaterialId);
                if (!hasMatchingMovement)
                {
                    throw new InvalidOperationException($"No uninvoiced material movements found for material id '{materialInput.MaterialId}'.");
                }
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            var now = DateTimeOffset.UtcNow;

            var invoice = new Invoice
            {
                InvoiceNumber = await GenerateInvoiceNumberAsync(dto.IssueDate, cancellationToken),
                Status = InvoiceStatus.DRAFT,
                ProjectId = project.Id,
                CustomerId = project.CustomerId,
                IssueDate = dto.IssueDate,
                DueDate = dto.DueDate,
                BillingPeriodStart = dto.BillingPeriodStart,
                BillingPeriodEnd = dto.BillingPeriodEnd,
                Currency = dto.Currency,
                Note = dto.Note,
                CreatedByUserId = createdByUserId,
                CustomerNameSnapshot = project.Customer.CompanyName,
                CustomerTaxIdSnapshot = project.Customer.TaxId,
                BillingAddressSnapshot = BuildAddressSnapshot(project.Customer.Address),
                CreatedAt = now,
                UpdatedAt = now
            };

            _dbContext.Invoices.Add(invoice);
            await _dbContext.SaveChangesAsync(cancellationToken);

            decimal subtotal = 0m;
            decimal tax = 0m;

            if (laborInput is not null)
            {
                var totalHours = workSessions.Sum(GetWorkSessionHours);

                var laborUnitPrice = laborInput.UnitPrice ?? 0m;

                var lineSubtotal = Math.Round(totalHours * laborUnitPrice, 2);
                var lineTax = Math.Round(lineSubtotal * laborInput.TaxRate, 2);
                var lineTotal = lineSubtotal + lineTax;

                var laborItem = new InvoiceLaborItem
                {
                    InvoiceId = invoice.Id,
                    Quantity = totalHours,
                    Unit = "h",
                    UnitPrice = laborUnitPrice,
                    TaxRate = laborInput.TaxRate,
                    LineSubtotal = lineSubtotal,
                    LineTax = lineTax,
                    LineTotal = lineTotal,
                    CreatedAt = now
                };

                _dbContext.InvoiceLaborItems.Add(laborItem);

                subtotal += lineSubtotal;
                tax += lineTax;

                foreach (var session in workSessions)
                {
                    session.InvoiceId = invoice.Id;
                    session.InvoicedAt = now;
                    session.UpdatedAt = now;
                }
            }

            foreach (var materialInput in materialInputs)
            {
                var matchingMovements = materialMovements
                    .Where(x => x.MaterialId == materialInput.MaterialId)
                    .ToList();

                var quantity = matchingMovements.Sum(x => x.Quantity);
                var material = matchingMovements.First().Material;
                var unit = matchingMovements.First().Unit;

                var materialId = materialInput.MaterialId!.Value;
                var unitPrice = allMaterials[materialId].Price;

                var lineSubtotal = Math.Round(quantity * unitPrice, 2);
                var lineTax = Math.Round(lineSubtotal * materialInput.TaxRate, 2);
                var lineTotal = lineSubtotal + lineTax;

                var materialItem = new InvoiceMaterialItem
                {
                    InvoiceId = invoice.Id,
                    MaterialId = material.Id,
                    Quantity = quantity,
                    Unit = unit,
                    UnitPrice = unitPrice,
                    TaxRate = materialInput.TaxRate,
                    LineSubtotal = lineSubtotal,
                    LineTax = lineTax,
                    LineTotal = lineTotal,
                    CreatedAt = now
                };

                _dbContext.InvoiceMaterialItems.Add(materialItem);

                subtotal += lineSubtotal;
                tax += lineTax;

                foreach (var movement in matchingMovements)
                {
                    movement.InvoiceId = invoice.Id;
                    movement.InvoicedAt = now;
                }
            }

            invoice.SubtotalAmount = Math.Round(subtotal, 2);
            invoice.TaxAmount = Math.Round(tax, 2);
            invoice.TotalAmount = invoice.SubtotalAmount + invoice.TaxAmount;
            invoice.UpdatedAt = now;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var created = await GetQuery()
                .FirstAsync(x => x.Id == invoice.Id, cancellationToken);

            return created.ToDetailsDto();
        }

        public async Task<IReadOnlyList<InvoiceListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default)
        {
            var invoices = await GetQuery()
                .OrderByDescending(x => x.IssueDate)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            return invoices
                .Select(x => x.ToListItemDto())
                .ToList();
        }

        public async Task<InvoiceDetailsDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var invoice = await GetQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (invoice is null)
            {
                throw new KeyNotFoundException($"Invoice with id '{id}' was not found.");
            }

            return invoice.ToDetailsDto();
        }

        public async Task<IReadOnlyList<InvoiceMaterialSelectOptionDto>> GetMaterialSelectOptionsAsync(
            long projectId,
            DateOnly billingPeriodStart,
            DateOnly billingPeriodEnd,
            CancellationToken cancellationToken = default)
        {
            if (billingPeriodEnd < billingPeriodStart)
            {
                throw new InvalidOperationException("Billing period end cannot be earlier than billing period start.");
            }

            var projectExists = await _dbContext.Projects
                .AnyAsync(x => x.Id == projectId, cancellationToken);

            if (!projectExists)
            {
                throw new KeyNotFoundException($"Project with id '{projectId}' was not found.");
            }

            var periodStart = new DateTimeOffset(
                billingPeriodStart.ToDateTime(TimeOnly.MinValue),
                TimeSpan.Zero);

            var periodEnd = new DateTimeOffset(
                billingPeriodEnd.ToDateTime(TimeOnly.MaxValue),
                TimeSpan.Zero);

            var rawItems = await _dbContext.MaterialMovements
                .AsNoTracking()
                .Include(x => x.Material)
                .Where(x =>
                    x.ProjectId == projectId &&
                    x.MovementType == MaterialMovementType.PROJECT_CONSUME &&
                    x.InvoiceId == null &&
                    x.InvoicedAt == null &&
                    x.OccurredAt >= periodStart &&
                    x.OccurredAt <= periodEnd)
                .GroupBy(x => new
                {
                    x.MaterialId,
                    x.Material.Name,
                    x.Unit
                })
                .Select(g => new
                {
                    MaterialId = g.Key.MaterialId,
                    MaterialName = g.Key.Name,
                    Unit = g.Key.Unit,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .OrderBy(x => x.MaterialName)
                .ToListAsync(cancellationToken);

            return rawItems
                .Select(x => new InvoiceMaterialSelectOptionDto
                {
                    MaterialId = x.MaterialId,
                    Label = $"{x.MaterialName} ({x.Quantity} {x.Unit})",
                    Quantity = x.Quantity,
                    Unit = x.Unit
                })
                .ToList();
        }

        public async Task<InvoiceDetailsDto> UpdateAsync(
            long id,
            UpdateInvoiceDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var invoice = await _dbContext.Invoices
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (invoice is null)
            {
                throw new KeyNotFoundException($"Invoice with id '{id}' was not found.");
            }

            if (invoice.Status != InvoiceStatus.DRAFT)
            {
                throw new InvalidOperationException("Only draft invoice can be updated.");
            }

            if (dto.DueDate < invoice.IssueDate)
            {
                throw new InvalidOperationException("DueDate cannot be earlier than IssueDate.");
            }

            invoice.DueDate = dto.DueDate;
            invoice.Note = dto.Note;
            invoice.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var updated = await GetQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailsDto();
        }

        public async Task<InvoiceDetailsDto> UpdateStatusAsync(
            long id,
            UpdateInvoiceStatusDto dto,
            CancellationToken cancellationToken = default)
        {
            var invoice = await _dbContext.Invoices
                .Include(x => x.WorkSessions)
                .Include(x => x.MaterialMovements)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (invoice is null)
            {
                throw new KeyNotFoundException($"Invoice with id '{id}' was not found.");
            }

            if (invoice.Status == dto.Status)
            {
                var sameStatusInvoice = await GetQuery()
                    .FirstAsync(x => x.Id == id, cancellationToken);

                return sameStatusInvoice.ToDetailsDto();
            }

            if (dto.Status == InvoiceStatus.CANCELED)
            {
                foreach (var session in invoice.WorkSessions)
                {
                    session.InvoiceId = null;
                    session.InvoicedAt = null;
                    session.UpdatedAt = DateTimeOffset.UtcNow;
                }

                foreach (var movement in invoice.MaterialMovements)
                {
                    movement.InvoiceId = null;
                    movement.InvoicedAt = null;
                }
            }

            invoice.Status = dto.Status;
            invoice.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var updated = await GetQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailsDto();
        }

        public async Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var invoice = await _dbContext.Invoices
                .Include(x => x.WorkSessions)
                .Include(x => x.MaterialMovements)
                .Include(x => x.LaborItem)
                .Include(x => x.MaterialItems)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (invoice is null)
            {
                throw new KeyNotFoundException($"Invoice with id '{id}' was not found.");
            }

            if (invoice.Status != InvoiceStatus.DRAFT)
            {
                throw new InvalidOperationException("Only draft invoice can be deleted.");
            }

            foreach (var session in invoice.WorkSessions)
            {
                session.InvoiceId = null;
                session.InvoicedAt = null;
                session.UpdatedAt = DateTimeOffset.UtcNow;
            }

            foreach (var movement in invoice.MaterialMovements)
            {
                movement.InvoiceId = null;
                movement.InvoicedAt = null;

            }

            _dbContext.Invoices.Remove(invoice);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<Invoice> GetQuery()
        {
            return _dbContext.Invoices
                .AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Customer)
                .Include(x => x.CreatedByUser)
                .Include(x => x.LaborItem)
                .Include(x => x.MaterialItems)
                    .ThenInclude(x => x.Material);
        }

        private async Task<string> GenerateInvoiceNumberAsync(DateOnly issueDate, CancellationToken cancellationToken)
        {
            var prefix = $"FV/{issueDate.Year}/{issueDate.Month:00}/";
            var count = await _dbContext.Invoices
                .IgnoreQueryFilters()
                .CountAsync(x => x.InvoiceNumber.StartsWith(prefix), cancellationToken);

            return $"{prefix}{count + 1:0000}";
        }

        private static decimal GetWorkSessionHours(Data.Models.WorkSessionModels.WorkSession session)
        {
            if (!session.EndedAt.HasValue)
            {
                return 0m;
            }

            var hours = (decimal)(session.EndedAt.Value - session.StartedAt).TotalHours;
            return Math.Round(hours, 2);
        }

        private static string BuildAddressSnapshot(Data.Models.AddressModels.Address? address)
        {
            if (address is null)
            {
                return string.Empty;
            }

            return string.Join(", ",
                new[]
                {
                    $"{address.Street} {address.BuildingNumber}{(string.IsNullOrWhiteSpace(address.ApartmentNumber) ? "" : $"/{address.ApartmentNumber}")}",
                    address.PostalCode,
                    address.City,
                    address.Country
                }.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        //public async Task<IReadOnlyList<InvoiceStatusSelectOptionDto>> GetSelectOptionsAsync(
        //               CancellationToken cancellationToken = default)
        //{
        //    var options = Enum.GetValues<InvoiceStatus>()
        //        .Select(x => new InvoiceStatusSelectOptionDto
        //        {
        //            Value = x,
        //            Label = x.ToString()
        //        })
        //        .ToList();
        //}
    }
}
