using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.PurchaseReqModels;
using PowerPulseRestAPI.Data.Models.StockRequestModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class Project
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public long? ResponsibleEmployeeId { get; set; }
        public User? CreatedByUser { get; set; }
        public Employee? ResponsibleEmployee { get; set; }

        public List<ProjectAccess> Accesses { get; set; } = new();
        public List<ProjectTask> Tasks { get; set; } = new();
        public List<ProjectNote> Notes { get; set; } = new();
        public List<ProjectAttachment> Attachments { get; set; } = new();
        public List<WorkSession> WorkSessions { get; set; } = new();
        public List<ProjectCustomer> ProjectCustomers { get; set; } = new();
        public List<ProjectCustomerContact> CustomerContacts { get; set; } = new();
        public List<MaterialMovement> MaterialMovements { get; set; } = new();
        public List<MaterialProjectBalance> MaterialBalances { get; set; } = new();
        public List<ToolAssignment> ToolAssignments { get; set; } = new();
        public List<PurchaseRequest> PurchaseRequests { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();
        public List<StockRequest> StockRequests { get; set; } = new();
        public List<Invoice> Invoices { get; set; } = new();
        public List<BillingRate> BillingRates { get; set; } = new();

    }

}
