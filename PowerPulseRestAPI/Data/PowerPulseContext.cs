using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;
using PowerPulseRestAPI.Data.Models.StockModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data.Models.AddressModels;
using PowerPulseRestAPI.Data.Models.MaterialsModels;
using PowerPulseRestAPI.Data.Models.WorkSessionModels;
using PowerPulseRestAPI.Data.Models.PersonModels;
using PowerPulseRestAPI.Data.Models.TextControlModels;

namespace PowerPulseRestAPI.Data
{
    public partial class PowerPulseContext : DbContext
    {
        public PowerPulseContext(DbContextOptions<PowerPulseContext>options) : base(options) { }

        // A) Users & auth
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();

        // B) HR
        public DbSet<Person> Persons => Set<Person>();
        public DbSet<PersonIdentifier> PersonIdentifiers => Set<PersonIdentifier>();
        public DbSet<Employee> Employees => Set<Employee>();


        // D) Projects
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectAccess> ProjectAccesses => Set<ProjectAccess>();
        public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();
        public DbSet<ProjectNote> ProjectNotes => Set<ProjectNote>();
        public DbSet<ProjectAttachment> ProjectAttachments => Set<ProjectAttachment>();

        // E) Work sessions
        public DbSet<WorkSession> WorkSessions => Set<WorkSession>();

        // F) Customers
        public DbSet<Customer> Customers => Set<Customer>();

        // G) Warehouse materials
        public DbSet<MaterialCategory> MaterialCategories => Set<MaterialCategory>();
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<MaterialMovement> MaterialMovements => Set<MaterialMovement>();

        // H) Tools
        public DbSet<ToolCategory> ToolCategories => Set<ToolCategory>();
        public DbSet<Tool> Tools => Set<Tool>();
        public DbSet<ToolAssignment> ToolAssignments => Set<ToolAssignment>();

        // J) Fleet
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<VehicleAssignment> VehicleAssignments => Set<VehicleAssignment>();
        public DbSet<VehicleIssue> VehicleIssues => Set<VehicleIssue>();


        // L) Knowledge base
        public DbSet<KnowledgeCategory> KnowledgeCategories => Set<KnowledgeCategory>();
        public DbSet<KnowledgeArticle> KnowledgeArticles => Set<KnowledgeArticle>();
        public DbSet<KnowledgeArticleAttachment> KnowledgeArticleAttachments => Set<KnowledgeArticleAttachment>();
        public DbSet<KnowledgeArticleFavorite> KnowledgeArticleFavorites => Set<KnowledgeArticleFavorite>();
        public DbSet<KnowledgeArticleRead> KnowledgeArticleReads => Set<KnowledgeArticleRead>();


        // N) Addresses
        public DbSet<Address> Addresses => Set<Address>();

        // O) Stock requests
        public DbSet<LowStockNote> LowStockNotes => Set<LowStockNote>();

        // P) Invoices
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceLaborItem> InvoiceLaborItems => Set<InvoiceLaborItem>();
        public DbSet<InvoiceMaterialItem> InvoiceMaterialItems => Set<InvoiceMaterialItem>();

        // Q) TextControlls
        public DbSet<TextTemplate> TextTemplates => Set<TextTemplate>();


    }
}
