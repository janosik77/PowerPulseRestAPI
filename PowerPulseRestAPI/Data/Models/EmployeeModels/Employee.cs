using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.StockRequestModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;

namespace PowerPulseRestAPI.Data.Models.EmployeeModels
{

    public class Employee
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public DateOnly HireDate { get; set; }
        public DateOnly? TerminatedAt { get; set; }
        public long PositionId { get; set; }
        public string? Department { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public EmployeeStatus Status { get; set; }
        public int RemainingVacationDays { get; set; }
        public int VacationDaysPerYear { get; set; }

        public Person? Person { get; set; }
        public Position? Position { get; set; }

        public List<EmployeeCompensation> Compensations { get; set; } = new();
        public List<EmployeeBankAccount> BankAccounts { get; set; } = new();

        public List<ProjectAccess> ProjectAccesses { get; set; } = new();
        public List<ProjectTask> AssignedTasks { get; set; } = new();
        public List<WorkSession> WorkSessions { get; set; } = new();
        public List<MaterialMovement> MaterialMovements { get; set; } = new();
        public List<ToolAssignment> ToolAssignments { get; set; } = new();
        public List<VehicleAssignment> VehicleAssignments { get; set; } = new();
        public List<StockRequest> StockRequests { get; set; } = new();



    }

}
