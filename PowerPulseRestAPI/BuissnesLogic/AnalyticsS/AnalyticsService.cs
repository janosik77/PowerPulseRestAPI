using System.Globalization;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.AnalyticsDto.Requests;
using PowerPulseRestAPI.DTO.AnalyticsDto.Responses;

namespace PowerPulseRestAPI.BuissnesLogic.AnalyticsS
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly PowerPulseContext _dbContext;

        public AnalyticsService(PowerPulseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AnalyticsOverviewDto> GetOverviewAsync(
            AnalyticsFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            filter ??= new AnalyticsFilterDto();

            var resolved = ResolveFilter(filter);

            var employeeProjectIds =
                await GetEmployeeProjectScopeAsync(
                    filter,
                    resolved,
                    cancellationToken);

            /*
             * WORK SESSIONS
             */
            var workSessionQuery = _dbContext.WorkSessions
                .AsNoTracking()
                .Where(x =>
                    x.StartedAt < resolved.ToExclusive &&
                    (
                        x.EndedAt == null ||
                        x.EndedAt >= resolved.FromDateTime
                    ));

            if (filter.ProjectId.HasValue)
            {
                workSessionQuery = workSessionQuery.Where(
                    x => x.ProjectId == filter.ProjectId.Value);
            }

            if (filter.CustomerId.HasValue)
            {
                workSessionQuery = workSessionQuery.Where(
                    x => x.Project.CustomerId == filter.CustomerId.Value);
            }

            if (filter.EmployeeId.HasValue)
            {
                workSessionQuery = workSessionQuery.Where(
                    x => x.EmployeeId == filter.EmployeeId.Value);
            }

            var workSessions = await workSessionQuery
                .Select(x => new WorkSessionAnalyticsRow
                {
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project.Name,

                    EmployeeId = x.EmployeeId,

                    EmployeeName =
                        x.Employee.Person.FirstName + " " +
                        x.Employee.Person.LastName,

                    StartedAt = x.StartedAt,
                    EndedAt = x.EndedAt
                })
                .ToListAsync(cancellationToken);

            /*
             * PROJECTS / PROJECT STATUS
             */
            var projectQuery = _dbContext.Projects
                .AsNoTracking()
                .Where(x =>
                    x.StartDate <= resolved.ToDate &&
                    (
                        x.EndDate == null ||
                        x.EndDate >= resolved.FromDate
                    ));

            if (filter.ProjectId.HasValue)
            {
                projectQuery = projectQuery.Where(
                    x => x.Id == filter.ProjectId.Value);
            }

            if (filter.CustomerId.HasValue)
            {
                projectQuery = projectQuery.Where(
                    x => x.CustomerId == filter.CustomerId.Value);
            }

            if (employeeProjectIds is not null)
            {
                projectQuery = projectQuery.Where(
                    x => employeeProjectIds.Contains(x.Id));
            }

            var projectStatuses = await projectQuery
                .Select(x => x.Status)
                .ToListAsync(cancellationToken);

            /*
             * INVOICES
             *
             * CANCELED invoices do not contribute to value.
             */
            var invoiceQuery = _dbContext.Invoices
                .AsNoTracking()
                .Where(x =>
                    x.IssueDate >= resolved.FromDate &&
                    x.IssueDate <= resolved.ToDate &&
                    x.Status != InvoiceStatus.CANCELED &&
                    x.Currency == resolved.Currency);

            if (filter.ProjectId.HasValue)
            {
                invoiceQuery = invoiceQuery.Where(
                    x => x.ProjectId == filter.ProjectId.Value);
            }

            if (filter.CustomerId.HasValue)
            {
                invoiceQuery = invoiceQuery.Where(
                    x => x.CustomerId == filter.CustomerId.Value);
            }

            if (employeeProjectIds is not null)
            {
                invoiceQuery = invoiceQuery.Where(
                    x => employeeProjectIds.Contains(x.ProjectId));
            }

            var invoices = await invoiceQuery
                .Select(x => new InvoiceAnalyticsRow
                {
                    IssueDate = x.IssueDate,
                    TotalAmount = x.TotalAmount
                })
                .ToListAsync(cancellationToken);

            /*
             * MATERIAL CONSUMPTION
             *
             * PROJECT_CONSUME                 => +
             * PROJECT_CONSUME_ADJUSTMENT...  => -
             */
            var materialQuery = _dbContext.MaterialMovements
                .AsNoTracking()
                .Where(x =>
                    x.ProjectId != null &&
                    x.OccurredAt >= resolved.FromDateTime &&
                    x.OccurredAt < resolved.ToExclusive &&
                    (
                        x.MovementType ==
                            MaterialMovementType.PROJECT_CONSUME ||
                        x.MovementType ==
                            MaterialMovementType
                                .PROJECT_CONSUME_ADJUSTMENT_DECREASE
                    ) &&
                    x.Material.Currency == resolved.Currency);

            if (filter.ProjectId.HasValue)
            {
                materialQuery = materialQuery.Where(
                    x => x.ProjectId == filter.ProjectId.Value);
            }

            if (filter.CustomerId.HasValue)
            {
                materialQuery = materialQuery.Where(
                    x => x.Project!.CustomerId == filter.CustomerId.Value);
            }

            if (employeeProjectIds is not null)
            {
                materialQuery = materialQuery.Where(
                    x =>
                        x.ProjectId.HasValue &&
                        employeeProjectIds.Contains(x.ProjectId.Value));
            }

            var materialRaw = await materialQuery
                .GroupBy(x => new
                {
                    x.MaterialId,
                    x.Material.Name,
                    x.Unit,
                    x.Material.Price,
                    x.Material.Currency
                })
                .Select(group => new
                {
                    MaterialId = group.Key.MaterialId,
                    MaterialName = group.Key.Name,
                    Unit = group.Key.Unit,
                    UnitPrice = group.Key.Price,
                    Currency = group.Key.Currency,

                    Quantity = group.Sum(x =>
                        x.MovementType ==
                        MaterialMovementType.PROJECT_CONSUME
                            ? x.Quantity
                            : -x.Quantity)
                })
                .ToListAsync(cancellationToken);

            var materialConsumption = materialRaw
                .Where(x => x.Quantity > 0)
                .Select(x => new MaterialConsumptionAnalyticsDto
                {
                    MaterialId = x.MaterialId,
                    MaterialName = x.MaterialName,
                    Quantity = x.Quantity,
                    Unit = x.Unit ?? string.Empty,
                    UnitPrice = x.UnitPrice,
                    TotalValue = Math.Round(
                        x.Quantity * x.UnitPrice,
                        2),
                    Currency = x.Currency
                })
                .OrderByDescending(x => x.TotalValue)
                .ToList();

            /*
             * DERIVED DATA
             */
            var totalWorkHours = workSessions.Sum(
                session => GetClippedHours(
                    session.StartedAt,
                    session.EndedAt,
                    resolved));

            var projectHours = workSessions
                .GroupBy(x => new
                {
                    x.ProjectId,
                    x.ProjectName
                })
                .Select(group => new ProjectHoursAnalyticsDto
                {
                    ProjectId = group.Key.ProjectId,
                    ProjectName = group.Key.ProjectName,

                    Hours = Math.Round(
                        group.Sum(x =>
                            GetClippedHours(
                                x.StartedAt,
                                x.EndedAt,
                                resolved)),
                        2)
                })
                .Where(x => x.Hours > 0)
                .OrderByDescending(x => x.Hours)
                .ToList();

            var employeeHours = workSessions
                .GroupBy(x => new
                {
                    x.EmployeeId,
                    x.EmployeeName
                })
                .Select(group => new EmployeeHoursAnalyticsDto
                {
                    EmployeeId = group.Key.EmployeeId,
                    EmployeeName = group.Key.EmployeeName,

                    Hours = Math.Round(
                        group.Sum(x =>
                            GetClippedHours(
                                x.StartedAt,
                                x.EndedAt,
                                resolved)),
                        2)
                })
                .Where(x => x.Hours > 0)
                .OrderByDescending(x => x.Hours)
                .ToList();

            var projectStatusDistribution = projectStatuses
                .GroupBy(x => x)
                .Select(group => new AnalyticsDistributionDto
                {
                    Key = group.Key.ToString(),
                    Label = HumanizeEnum(group.Key.ToString()),
                    Value = group.Count()
                })
                .OrderByDescending(x => x.Value)
                .ToList();

            var workHoursTimeline =
                BuildWorkHoursTimeline(
                    workSessions,
                    resolved);

            var invoiceTimeline =
                BuildInvoiceTimeline(
                    invoices,
                    resolved);

            return new AnalyticsOverviewDto
            {
                Period = new AnalyticsPeriodDto
                {
                    From = resolved.FromDate,
                    To = resolved.ToDate,
                    Granularity =
                        resolved.Granularity.ToString().ToLowerInvariant(),
                    Currency = resolved.Currency
                },

                Summary = new AnalyticsSummaryDto
                {
                    ActiveProjects = projectStatuses.Count(
                        x => x == ProjectStatus.ACTIVE),

                    WorkHours = Math.Round(
                        totalWorkHours,
                        2),

                    InvoiceValue = Math.Round(
                        invoices.Sum(x => x.TotalAmount),
                        2),

                    MaterialCost = Math.Round(
                        materialConsumption.Sum(x => x.TotalValue),
                        2),

                    Currency = resolved.Currency
                },

                WorkHoursTimeline = workHoursTimeline,

                ProjectHours = projectHours,

                ProjectStatuses = projectStatusDistribution,

                MaterialConsumption = materialConsumption,

                InvoiceTimeline = invoiceTimeline,

                EmployeeHours = employeeHours
            };
        }

        public async Task<AnalyticsFilterOptionsDto> GetFilterOptionsAsync(
            CancellationToken cancellationToken = default)
        {
            var projects = await _dbContext.Projects
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new AnalyticsSelectOptionDto
                {
                    Value = x.Id,

                    Label =
                        x.Code + " · " + x.Name
                })
                .ToListAsync(cancellationToken);

            var employees = await _dbContext.Employees
                .AsNoTracking()
                .OrderBy(x => x.Person.LastName)
                .ThenBy(x => x.Person.FirstName)
                .Select(x => new AnalyticsSelectOptionDto
                {
                    Value = x.Id,

                    Label =
                        x.Person.FirstName + " " +
                        x.Person.LastName
                })
                .ToListAsync(cancellationToken);

            var customers = await _dbContext.Customers
                .AsNoTracking()
                .OrderBy(x => x.CompanyName)
                .Select(x => new AnalyticsSelectOptionDto
                {
                    Value = x.Id,
                    Label = x.CompanyName
                })
                .ToListAsync(cancellationToken);

            var invoiceCurrencies = await _dbContext.Invoices
                .AsNoTracking()
                .Select(x => x.Currency)
                .Distinct()
                .ToListAsync(cancellationToken);

            var materialCurrencies = await _dbContext.Materials
                .AsNoTracking()
                .Select(x => x.Currency)
                .Distinct()
                .ToListAsync(cancellationToken);

            var currencies = invoiceCurrencies
                .Concat(materialCurrencies)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToUpperInvariant())
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            if (currencies.Count == 0)
            {
                currencies.Add("PLN");
            }

            return new AnalyticsFilterOptionsDto
            {
                Projects = projects,
                Employees = employees,
                Customers = customers,
                Currencies = currencies
            };
        }

        /*
         * EMPLOYEE FILTER
         *
         * For finance/material analytics Employee does not mean
         * "created by employee".
         *
         * It means:
         * projects in which this employee worked during selected period.
         *
         * This keeps the global employee filter meaningful across the
         * whole dashboard.
         */
        private async Task<List<long>?> GetEmployeeProjectScopeAsync(
            AnalyticsFilterDto filter,
            ResolvedAnalyticsFilter resolved,
            CancellationToken cancellationToken)
        {
            if (!filter.EmployeeId.HasValue)
            {
                return null;
            }

            var query = _dbContext.WorkSessions
                .AsNoTracking()
                .Where(x =>
                    x.EmployeeId == filter.EmployeeId.Value &&
                    x.StartedAt < resolved.ToExclusive &&
                    (
                        x.EndedAt == null ||
                        x.EndedAt >= resolved.FromDateTime
                    ));

            if (filter.ProjectId.HasValue)
            {
                query = query.Where(
                    x => x.ProjectId == filter.ProjectId.Value);
            }

            if (filter.CustomerId.HasValue)
            {
                query = query.Where(
                    x => x.Project.CustomerId == filter.CustomerId.Value);
            }

            return await query
                .Select(x => x.ProjectId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        private static ResolvedAnalyticsFilter ResolveFilter(
            AnalyticsFilterDto filter)
        {
            var today = DateOnly.FromDateTime(
                DateTime.UtcNow);

            var from = filter.From ?? today.AddDays(-29);

            var to = filter.To ?? today;

            if (to < from)
            {
                throw new InvalidOperationException(
                    "'To' cannot be earlier than 'From'.");
            }

            var days =
                to.DayNumber -
                from.DayNumber +
                1;

            var granularity =
                days <= 45
                    ? AnalyticsGranularity.Day
                    : days <= 180
                        ? AnalyticsGranularity.Week
                        : AnalyticsGranularity.Month;

            var currency =
                string.IsNullOrWhiteSpace(filter.Currency)
                    ? "PLN"
                    : filter.Currency.Trim().ToUpperInvariant();

            var fromDateTime = new DateTimeOffset(
                from.ToDateTime(TimeOnly.MinValue),
                TimeSpan.Zero);

            var toExclusive = new DateTimeOffset(
                to.AddDays(1).ToDateTime(TimeOnly.MinValue),
                TimeSpan.Zero);

            return new ResolvedAnalyticsFilter
            {
                FromDate = from,
                ToDate = to,
                FromDateTime = fromDateTime,
                ToExclusive = toExclusive,
                Currency = currency,
                Granularity = granularity
            };
        }

        private static decimal GetClippedHours(
            DateTimeOffset startedAt,
            DateTimeOffset? endedAt,
            ResolvedAnalyticsFilter filter)
        {
            var end = endedAt ?? DateTimeOffset.UtcNow;

            var clippedStart =
                startedAt > filter.FromDateTime
                    ? startedAt
                    : filter.FromDateTime;

            var clippedEnd =
                end < filter.ToExclusive
                    ? end
                    : filter.ToExclusive;

            if (clippedEnd <= clippedStart)
            {
                return 0m;
            }

            return (decimal)
                (clippedEnd - clippedStart)
                .TotalHours;
        }

        private static IReadOnlyList<AnalyticsTimePointDto>
            BuildWorkHoursTimeline(
                IReadOnlyList<WorkSessionAnalyticsRow> sessions,
                ResolvedAnalyticsFilter filter)
        {
            var buckets = CreateBuckets(filter);

            foreach (var session in sessions)
            {
                var end =
                    session.EndedAt ??
                    DateTimeOffset.UtcNow;

                var current =
                    session.StartedAt > filter.FromDateTime
                        ? session.StartedAt
                        : filter.FromDateTime;

                var clippedEnd =
                    end < filter.ToExclusive
                        ? end
                        : filter.ToExclusive;

                while (current < clippedEnd)
                {
                    var currentDate =
                        DateOnly.FromDateTime(
                            current.UtcDateTime);

                    var bucketDate =
                        GetBucketStart(
                            currentDate,
                            filter.Granularity);

                    var nextBucketDate =
                        GetNextBucketStart(
                            bucketDate,
                            filter.Granularity);

                    var nextBucketDateTime =
                        new DateTimeOffset(
                            nextBucketDate.ToDateTime(
                                TimeOnly.MinValue),
                            TimeSpan.Zero);

                    var segmentEnd =
                        clippedEnd < nextBucketDateTime
                            ? clippedEnd
                            : nextBucketDateTime;

                    var hours =
                        (decimal)
                        (segmentEnd - current)
                        .TotalHours;

                    if (buckets.ContainsKey(bucketDate))
                    {
                        buckets[bucketDate] += hours;
                    }

                    current = segmentEnd;
                }
            }

            return buckets
                .OrderBy(x => x.Key)
                .Select(x => new AnalyticsTimePointDto
                {
                    Date = x.Key,
                    Value = Math.Round(x.Value, 2)
                })
                .ToList();
        }

        private static IReadOnlyList<AnalyticsTimePointDto>
            BuildInvoiceTimeline(
                IReadOnlyList<InvoiceAnalyticsRow> invoices,
                ResolvedAnalyticsFilter filter)
        {
            var buckets = CreateBuckets(filter);

            foreach (var invoice in invoices)
            {
                var bucketDate =
                    GetBucketStart(
                        invoice.IssueDate,
                        filter.Granularity);

                if (buckets.ContainsKey(bucketDate))
                {
                    buckets[bucketDate] +=
                        invoice.TotalAmount;
                }
            }

            return buckets
                .OrderBy(x => x.Key)
                .Select(x => new AnalyticsTimePointDto
                {
                    Date = x.Key,
                    Value = Math.Round(x.Value, 2)
                })
                .ToList();
        }

        private static Dictionary<DateOnly, decimal>
            CreateBuckets(
                ResolvedAnalyticsFilter filter)
        {
            var result =
                new Dictionary<DateOnly, decimal>();

            var current =
                GetBucketStart(
                    filter.FromDate,
                    filter.Granularity);

            while (current <= filter.ToDate)
            {
                result[current] = 0m;

                current =
                    GetNextBucketStart(
                        current,
                        filter.Granularity);
            }

            return result;
        }

        private static DateOnly GetBucketStart(
            DateOnly date,
            AnalyticsGranularity granularity)
        {
            return granularity switch
            {
                AnalyticsGranularity.Day =>
                    date,

                AnalyticsGranularity.Week =>
                    date.AddDays(
                        -(((int)date.DayOfWeek + 6) % 7)),

                AnalyticsGranularity.Month =>
                    new DateOnly(
                        date.Year,
                        date.Month,
                        1),

                _ => date
            };
        }

        private static DateOnly GetNextBucketStart(
            DateOnly current,
            AnalyticsGranularity granularity)
        {
            return granularity switch
            {
                AnalyticsGranularity.Day =>
                    current.AddDays(1),

                AnalyticsGranularity.Week =>
                    current.AddDays(7),

                AnalyticsGranularity.Month =>
                    current.AddMonths(1),

                _ => current.AddDays(1)
            };
        }

        private static string HumanizeEnum(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var text =
                value
                    .Replace("_", " ")
                    .ToLowerInvariant();

            return CultureInfo
                .InvariantCulture
                .TextInfo
                .ToTitleCase(text);
        }

        private sealed class WorkSessionAnalyticsRow
        {
            public long ProjectId { get; set; }

            public string ProjectName { get; set; }
                = string.Empty;

            public long EmployeeId { get; set; }

            public string EmployeeName { get; set; }
                = string.Empty;

            public DateTimeOffset StartedAt { get; set; }

            public DateTimeOffset? EndedAt { get; set; }
        }

        private sealed class InvoiceAnalyticsRow
        {
            public DateOnly IssueDate { get; set; }

            public decimal TotalAmount { get; set; }
        }

        private sealed class ResolvedAnalyticsFilter
        {
            public DateOnly FromDate { get; init; }

            public DateOnly ToDate { get; init; }

            public DateTimeOffset FromDateTime { get; init; }

            public DateTimeOffset ToExclusive { get; init; }

            public string Currency { get; init; }
                = "PLN";

            public AnalyticsGranularity Granularity { get; init; }
        }

        private enum AnalyticsGranularity
        {
            Day,
            Week,
            Month
        }
    }
}