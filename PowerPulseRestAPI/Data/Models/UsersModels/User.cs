using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.PurchaseReqModels;
using PowerPulseRestAPI.Data.Models.StockRequestModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;

namespace PowerPulseRestAPI.Data.Models.UsersModels
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTimeOffset? LastPasswordUpdate { get; set; }
        public DateTimeOffset? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Person? Person { get; set; }
        public List<TaskUpdate> TaskUpdates { get; set; } = new();
        public List<WorkSession> StartedWorkSessions { get; set; } = new();
        public List<WorkSessionEvent> WorkSessionEvents { get; set; } = new();
        public List<CustomerNote> CustomerNotes { get; set; } = new();
        public List<MaterialMovement> CreatedMaterialMovements { get; set; } = new();
        public List<ToolAssignment> CreatedToolAssignments { get; set; } = new();
        public List<ToolIssue> ReportedToolIssues { get; set; } = new();
        public List<PurchaseRequest> PurchaseRequestsCreated { get; set; } = new();
        public List<PurchaseRequest> PurchaseRequestsApproved { get; set; } = new();
        public List<VehicleMileageRecord> VehicleMileageRecords { get; set; } = new();
        public List<ServiceOrder> ServiceOrdersRequested { get; set; } = new();
        public List<ServiceOrder> ServiceOrdersApproved { get; set; } = new();
        public List<ServiceOrder> ServiceOrdersCompleted { get; set; } = new();
        public List<ServiceOrderHistory> ServiceOrderChanges { get; set; } = new();
        public List<VehicleIssue> VehicleIssuesReported { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();
        public List<Notification> CreatedNotifications { get; set; } = new();
        public List<ActivityLog> ActivityLogs { get; set; } = new();
        public List<KnowledgeArticle> KnowledgeArticlesCreated { get; set; } = new();
        public List<KnowledgeArticle> KnowledgeArticlesUpdated { get; set; } = new();
        public List<KnowledgeArticleFavorite> KnowledgeFavorites { get; set; } = new();
        public List<KnowledgeArticleRead> KnowledgeReads { get; set; } = new();
        public List<StockRequest> StockRequestsRequested { get; set; } = new();
        public List<StockRequest> StockRequestsApproved { get; set; } = new();
        public List<StockRequestFulfillment> StockRequestFulfillmentsIssued { get; set; } = new();
        public List<StockRequestHistory> StockRequestChanges { get; set; } = new();
        public List<StockRequestReservation> StockRequestReservations { get; set; } = new();

        public List<Invoice> InvoicesCreated { get; set; } = new();
        public List<Invoice> InvoicesIssued { get; set; } = new();
        public List<InvoiceHistory> InvoiceChanges { get; set; } = new();


    }
}
