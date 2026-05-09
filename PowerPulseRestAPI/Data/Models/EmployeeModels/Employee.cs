using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.PersonModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.StockModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;
using PowerPulseRestAPI.Data.Models.WorkSessionModels;

namespace PowerPulseRestAPI.Data.Models.EmployeeModels
{
    public class Employee
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public DateOnly HireDate { get; set; }
        public DateOnly? TerminatedAt { get; set; }
        public string? Department { get; set; }
        public string JobTitle { get; set; } = null!;
        public decimal HourlyWage { get; set; }
        public string Currency { get; set; } = "PLN";
        public EmployeeStatus Status { get; set; }
        public int RemainingVacationDays { get; set; }
        public int VacationDaysPerYear { get; set; }
        public string AccountEncrypted { get; set; } = null!;
        public string? AccountLast4 { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public Person Person { get; set; } = null!;
        public List<ProjectAccess> ProjectAccesses { get; set; } = new();
        public List<ProjectTask> AssignedTasks { get; set; } = new();
        public List<WorkSession> WorkSessions { get; set; } = new();
        public List<ToolAssignment> ToolAssignments { get; set; } = new();
        public List<VehicleAssignment>? VehicleAssignments { get; set; }
        public List<ProjectNote> ProjectNotes { get; set; } = new();
        public List<LowStockNote> LowStockNotes { get; set; } = new();
    }

}
