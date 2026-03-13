using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;
using PowerPulseRestAPI.Data.Models.MaterialsModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
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
        public long RoleId { get; set; }
        public long PersonId { get; set; }
        public DateTimeOffset? LastPasswordUpdate { get; set; }
        public DateTimeOffset? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Role Role { get; set; } = null!;
        public Person Person { get; set; } = null!;
        public List<TaskUpdate> TaskUpdates { get; set; } = new();
        public List<WorkSession> StartedWorkSessions { get; set; } = new();
        public List<CustomerNote> CustomerNotes { get; set; } = new();
        public List<MaterialMovement> CreatedMaterialMovements { get; set; } = new();
        public List<ToolAssignment> CreatedToolAssignments { get; set; } = new();
        public List<ToolIssue> ReportedToolIssues { get; set; } = new();
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
        public List<StockRequest> StockRequestsReceivedConfirmed { get; set; } = new();
        public List<StockRequestHistory> StockRequestChanges { get; set; } = new();
        public List<MaterialProjectConsume> CreatedMaterialProjectConsumes { get; set; } = new();
        public List<Invoice> InvoicesCreated { get; set; } = new();
        public List<Invoice> InvoicesIssued { get; set; } = new();
        public List<InvoiceHistory> InvoiceChanges { get; set; } = new();


    }
}
