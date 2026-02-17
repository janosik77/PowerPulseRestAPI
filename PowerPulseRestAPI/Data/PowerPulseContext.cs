using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models;
using System.Data;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;
using PowerPulseRestAPI.Data.Models.StockRequestModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data.Models.PurchaseReqModels;
using PowerPulseRestAPI.Data.Models.AddressModels;

namespace PowerPulseRestAPI.Data
{
    public partial class PowerPulseContext : DbContext
    {
        public PowerPulseContext(DbContextOptions<PowerPulseContext>options) : base(options) { }

        // A) Users & auth
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        // B) HR
        public DbSet<Person> Persons => Set<Person>();
        public DbSet<PersonIdentifier> PersonIdentifiers => Set<PersonIdentifier>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<EmployeeCompensation> EmployeeCompensations => Set<EmployeeCompensation>();
        public DbSet<EmployeeBankAccount> EmployeeBankAccounts => Set<EmployeeBankAccount>();

        // C) Leaves
        public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

        // D) Projects
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectAccess> ProjectAccesses => Set<ProjectAccess>();
        public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();
        public DbSet<TaskUpdate> TaskUpdates => Set<TaskUpdate>();
        public DbSet<TaskAttachment> TaskAttachments => Set<TaskAttachment>();
        public DbSet<ProjectNote> ProjectNotes => Set<ProjectNote>();
        public DbSet<ProjectAttachment> ProjectAttachments => Set<ProjectAttachment>();

        // E) Work sessions
        public DbSet<WorkSession> WorkSessions => Set<WorkSession>();
        public DbSet<WorkSessionEvent> WorkSessionEvents => Set<WorkSessionEvent>();

        // F) Customers
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<CustomerContact> CustomerContacts => Set<CustomerContact>();
        public DbSet<ProjectCustomer> ProjectCustomers => Set<ProjectCustomer>();
        public DbSet<ProjectCustomerContact> ProjectCustomerContacts => Set<ProjectCustomerContact>();
        public DbSet<CustomerNote> CustomerNotes => Set<CustomerNote>();

        // G) Warehouse materials
        public DbSet<StorageLocation> StorageLocations => Set<StorageLocation>();
        public DbSet<MaterialCategory> MaterialCategories => Set<MaterialCategory>();
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<MaterialImage> MaterialImages => Set<MaterialImage>();
        public DbSet<MaterialStock> MaterialStocks => Set<MaterialStock>();
        public DbSet<MaterialMovement> MaterialMovements => Set<MaterialMovement>();
        public DbSet<MaterialProjectBalance> MaterialProjectBalances => Set<MaterialProjectBalance>();
        public DbSet<MaterialVehicleBalance> MaterialVehicleBalances => Set<MaterialVehicleBalance>();

        // H) Tools
        public DbSet<ToolCategory> ToolCategories => Set<ToolCategory>();
        public DbSet<Tool> Tools => Set<Tool>();
        public DbSet<ToolImage> ToolImages => Set<ToolImage>();
        public DbSet<ToolAsset> ToolAssets => Set<ToolAsset>();
        public DbSet<ToolAssetStock> ToolAssetStocks => Set<ToolAssetStock>();
        public DbSet<ToolAssignment> ToolAssignments => Set<ToolAssignment>();
        public DbSet<ToolIssue> ToolIssues => Set<ToolIssue>();
        public DbSet<ToolIssueAttachment> ToolIssueAttachments => Set<ToolIssueAttachment>();

        // I) Purchases
        public DbSet<PurchaseRequest> PurchaseRequests => Set<PurchaseRequest>();
        public DbSet<PurchaseRequestItem> PurchaseRequestItems => Set<PurchaseRequestItem>();

        // J) Fleet
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<VehicleAssignment> VehicleAssignments => Set<VehicleAssignment>();
        public DbSet<VehicleMileageRecord> VehicleMileageRecords => Set<VehicleMileageRecord>();
        public DbSet<ServiceOrder> ServiceOrders => Set<ServiceOrder>();
        public DbSet<ServiceOrderHistory> ServiceOrderHistories => Set<ServiceOrderHistory>();
        public DbSet<VehicleIssue> VehicleIssues => Set<VehicleIssue>();
        public DbSet<VehicleIssueAttachment> VehicleIssueAttachments => Set<VehicleIssueAttachment>();

        // K) Notifications & logs
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

        // L) Knowledge base
        public DbSet<KnowledgeCategory> KnowledgeCategories => Set<KnowledgeCategory>();
        public DbSet<KnowledgeArticle> KnowledgeArticles => Set<KnowledgeArticle>();
        public DbSet<KnowledgeArticleAttachment> KnowledgeArticleAttachments => Set<KnowledgeArticleAttachment>();
        public DbSet<KnowledgeArticleFavorite> KnowledgeArticleFavorites => Set<KnowledgeArticleFavorite>();
        public DbSet<KnowledgeArticleRead> KnowledgeArticleReads => Set<KnowledgeArticleRead>();

        // M) Calculators
        public DbSet<Calculator> Calculators => Set<Calculator>();

        // N) Addresses
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<EntityAddress> EntityAddresses => Set<EntityAddress>();

        // O) Stock requests
        public DbSet<StockRequest> StockRequests => Set<StockRequest>();
        public DbSet<StockRequestItem> StockRequestItems => Set<StockRequestItem>();
        public DbSet<StockRequestFulfillment> StockRequestFulfillments => Set<StockRequestFulfillment>();
        public DbSet<StockRequestFulfillmentItem> StockRequestFulfillmentItems => Set<StockRequestFulfillmentItem>();
        public DbSet<StockRequestHistory> StockRequestHistories => Set<StockRequestHistory>();
        public DbSet<StockRequestReservation> StockRequestReservations => Set<StockRequestReservation>();

        // P) Invoices
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
        public DbSet<InvoiceItemSource> InvoiceItemSources => Set<InvoiceItemSource>();
        public DbSet<InvoiceHistory> InvoiceHistories => Set<InvoiceHistory>();
        public DbSet<BillingRate> BillingRates => Set<BillingRate>();
        public DbSet<MaterialPriceList> MaterialPriceLists => Set<MaterialPriceList>();

    }
}
