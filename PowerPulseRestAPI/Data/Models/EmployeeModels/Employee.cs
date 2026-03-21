using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.StockRequestModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;

namespace PowerPulseRestAPI.Data.Models.EmployeeModels
{
    //usunięto Position również tabelę więc konieczna migracja
    public class Employee
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public DateOnly HireDate { get; set; }
        public DateOnly? TerminatedAt { get; set; }
        public string? Department { get; set; }
        public string JobTitle { get; set; } = null!;
        public EmployeeStatus Status { get; set; }
        public int RemainingVacationDays { get; set; }
        public int VacationDaysPerYear { get; set; }
        public string AccountEncrypted { get; set; } = null!;
        public string? AccountLast4 { get; set; }
        
        public Person Person { get; set; } = null!;
        public List<EmployeeCompensation> Compensations { get; set; } = new();
        public List<ProjectAccess> ProjectAccesses { get; set; } = new();
        public List<ProjectTask> AssignedTasks { get; set; } = new();
        public List<WorkSession> WorkSessions { get; set; } = new();
        public List<ToolAssignment> ToolAssignments { get; set; } = new();
        public List<VehicleAssignment> VehicleAssignments { get; set; } = new();

    }

}
