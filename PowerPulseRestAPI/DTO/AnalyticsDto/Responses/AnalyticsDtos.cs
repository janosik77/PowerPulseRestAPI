namespace PowerPulseRestAPI.DTO.AnalyticsDto.Responses
{
    public class AnalyticsOverviewDto
    {
        public AnalyticsPeriodDto Period { get; set; } = new();

        public AnalyticsSummaryDto Summary { get; set; } = new();

        public IReadOnlyList<AnalyticsTimePointDto> WorkHoursTimeline { get; set; }
            = [];

        public IReadOnlyList<ProjectHoursAnalyticsDto> ProjectHours { get; set; }
            = [];

        public IReadOnlyList<AnalyticsDistributionDto> ProjectStatuses { get; set; }
            = [];

        public IReadOnlyList<MaterialConsumptionAnalyticsDto> MaterialConsumption { get; set; }
            = [];

        public IReadOnlyList<AnalyticsTimePointDto> InvoiceTimeline { get; set; }
            = [];

        public IReadOnlyList<EmployeeHoursAnalyticsDto> EmployeeHours { get; set; }
            = [];
    }

    public class AnalyticsPeriodDto
    {
        public DateOnly From { get; set; }

        public DateOnly To { get; set; }

        public string Granularity { get; set; } = "day";

        public string Currency { get; set; } = "PLN";
    }

    public class AnalyticsSummaryDto
    {
        public int ActiveProjects { get; set; }

        public decimal WorkHours { get; set; }

        public decimal InvoiceValue { get; set; }

        public decimal MaterialCost { get; set; }

        public string Currency { get; set; } = "PLN";
    }

    public class AnalyticsTimePointDto
    {
        public DateOnly Date { get; set; }

        public decimal Value { get; set; }
    }

    public class ProjectHoursAnalyticsDto
    {
        public long ProjectId { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public decimal Hours { get; set; }
    }

    public class EmployeeHoursAnalyticsDto
    {
        public long EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public decimal Hours { get; set; }
    }

    public class AnalyticsDistributionDto
    {
        public string Key { get; set; } = string.Empty;

        public string Label { get; set; } = string.Empty;

        public decimal Value { get; set; }
    }

    public class MaterialConsumptionAnalyticsDto
    {
        public long MaterialId { get; set; }

        public string MaterialName { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public decimal TotalValue { get; set; }

        public string Currency { get; set; } = string.Empty;
    }

    public class AnalyticsFilterOptionsDto
    {
        public IReadOnlyList<AnalyticsSelectOptionDto> Projects { get; set; }
            = [];

        public IReadOnlyList<AnalyticsSelectOptionDto> Employees { get; set; }
            = [];

        public IReadOnlyList<AnalyticsSelectOptionDto> Customers { get; set; }
            = [];

        public IReadOnlyList<string> Currencies { get; set; }
            = [];
    }

    public class AnalyticsSelectOptionDto
    {
        public long Value { get; set; }

        public string Label { get; set; } = string.Empty;
    }
}