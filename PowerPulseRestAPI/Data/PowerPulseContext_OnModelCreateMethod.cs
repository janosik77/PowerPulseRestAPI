using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System;
using PowerPulseRestAPI.Data.Models;
using PowerPulseRestAPI.Data.Enums;
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
        protected override void OnModelCreating(ModelBuilder b)
        {
            // Helper: common
            b.Entity<User>().ToTable("users");
            b.Entity<Role>().ToTable("roles");
            b.Entity<UserRole>().ToTable("user_roles");
            b.Entity<Person>().ToTable("person");
            b.Entity<PersonIdentifier>().ToTable("person_identifiers");
            b.Entity<Position>().ToTable("positions");
            b.Entity<Employee>().ToTable("employee");
            b.Entity<EmployeeCompensation>().ToTable("employee_compensations");
            b.Entity<EmployeeBankAccount>().ToTable("employee_bank_accounts");

            b.Entity<LeaveRequest>().ToTable("leave_requests");

            b.Entity<Project>().ToTable("projects");
            b.Entity<ProjectAccess>().ToTable("project_access");
            b.Entity<ProjectTask>().ToTable("project_tasks");
            b.Entity<TaskUpdate>().ToTable("task_updates");
            b.Entity<TaskAttachment>().ToTable("task_attachments");
            b.Entity<ProjectNote>().ToTable("project_notes");
            b.Entity<ProjectAttachment>().ToTable("project_attachments");

            b.Entity<WorkSession>().ToTable("work_sessions");
            b.Entity<WorkSessionEvent>().ToTable("work_session_events");

            b.Entity<Customer>().ToTable("customers");
            b.Entity<CustomerContact>().ToTable("customer_contacts");
            b.Entity<ProjectCustomer>().ToTable("project_customers");
            b.Entity<ProjectCustomerContact>().ToTable("project_customer_contacts");
            b.Entity<CustomerNote>().ToTable("customer_notes");

            b.Entity<StorageLocation>().ToTable("storage_locations");
            b.Entity<MaterialCategory>().ToTable("material_categories");
            b.Entity<Material>().ToTable("materials");
            b.Entity<MaterialImage>().ToTable("material_images");
            b.Entity<MaterialStock>().ToTable("material_stock");
            b.Entity<MaterialMovement>().ToTable("material_movements");
            b.Entity<MaterialProjectBalance>().ToTable("material_project_balance");
            b.Entity<MaterialVehicleBalance>().ToTable("material_vehicle_balance");

            b.Entity<ToolCategory>().ToTable("tool_categories");
            b.Entity<Tool>().ToTable("tools");
            b.Entity<ToolImage>().ToTable("tool_images");
            b.Entity<ToolAsset>().ToTable("tool_assets");
            b.Entity<ToolAssetStock>().ToTable("tool_asset_stock");
            b.Entity<ToolAssignment>().ToTable("tool_assignments");
            b.Entity<ToolIssue>().ToTable("tool_issues");
            b.Entity<ToolIssueAttachment>().ToTable("tool_issue_attachments");

            b.Entity<PurchaseRequest>().ToTable("purchase_requests");
            b.Entity<PurchaseRequestItem>().ToTable("purchase_request_items");

            b.Entity<Vehicle>().ToTable("vehicles");
            b.Entity<VehicleAssignment>().ToTable("vehicle_assignments");
            b.Entity<VehicleMileageRecord>().ToTable("vehicle_mileage_records");
            b.Entity<ServiceOrder>().ToTable("service_orders");
            b.Entity<ServiceOrderHistory>().ToTable("service_order_history");
            b.Entity<VehicleIssue>().ToTable("vehicle_issues");
            b.Entity<VehicleIssueAttachment>().ToTable("vehicle_issue_attachments");

            b.Entity<Notification>().ToTable("notifications");
            b.Entity<ActivityLog>().ToTable("activity_log");

            b.Entity<KnowledgeCategory>().ToTable("knowledge_categories");
            b.Entity<KnowledgeArticle>().ToTable("knowledge_articles");
            b.Entity<KnowledgeArticleAttachment>().ToTable("knowledge_article_attachments");
            b.Entity<KnowledgeArticleFavorite>().ToTable("knowledge_article_favorites");
            b.Entity<KnowledgeArticleRead>().ToTable("knowledge_article_reads");

            b.Entity<Calculator>().ToTable("calculators");

            b.Entity<Address>().ToTable("addresses");
            b.Entity<EntityAddress>().ToTable("entity_addresses");

            b.Entity<StockRequest>().ToTable("stock_requests");
            b.Entity<StockRequestItem>().ToTable("stock_request_items");
            b.Entity<StockRequestFulfillment>().ToTable("stock_request_fulfillments");
            b.Entity<StockRequestFulfillmentItem>().ToTable("stock_request_fulfillment_items");
            b.Entity<StockRequestHistory>().ToTable("stock_request_history");
            b.Entity<StockRequestReservation>().ToTable("stock_request_reservations");

            b.Entity<Invoice>().ToTable("invoices");
            b.Entity<InvoiceItem>().ToTable("invoice_items");
            b.Entity<InvoiceItemSource>().ToTable("invoice_item_sources");
            b.Entity<InvoiceHistory>().ToTable("invoice_history");
            b.Entity<BillingRate>().ToTable("billing_rates");
            b.Entity<MaterialPriceList>().ToTable("material_price_lists");

            // Composite keys
            b.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleId });
            b.Entity<ProjectCustomer>().HasKey(x => new { x.ProjectId, x.CustomerId, x.RelationshipType });
            b.Entity<ProjectCustomerContact>().HasKey(x => new { x.ProjectId, x.CustomerContactId, x.ContactRole });

            // Unique constraints
            b.Entity<User>().HasIndex(x => x.Email).IsUnique();
            b.Entity<User>().HasIndex(x => x.Login).IsUnique();
            b.Entity<Role>().HasIndex(x => x.Name).IsUnique();
            b.Entity<PersonIdentifier>().HasIndex(x => new { x.PersonId, x.Type }).IsUnique();
            b.Entity<Project>().HasIndex(x => x.Code).IsUnique();
            b.Entity<StorageLocation>().HasIndex(x => x.Code).IsUnique();
            b.Entity<MaterialCategory>().HasIndex(x => x.Name).IsUnique();
            b.Entity<Material>().HasIndex(x => x.Sku).IsUnique();
            b.Entity<Material>().HasIndex(x => x.Barcode).IsUnique();
            b.Entity<ToolCategory>().HasIndex(x => x.Name).IsUnique();
            b.Entity<Tool>().HasIndex(x => x.Sku).IsUnique();
            b.Entity<Tool>().HasIndex(x => x.Barcode).IsUnique();
            b.Entity<ToolAsset>().HasIndex(x => x.SerialNumber).IsUnique();
            b.Entity<ToolAsset>().HasIndex(x => x.AssetTag).IsUnique();
            b.Entity<KnowledgeCategory>().HasIndex(x => x.Name).IsUnique();
            b.Entity<Calculator>().HasIndex(x => x.Code).IsUnique();
            b.Entity<Invoice>().HasIndex(x => x.InvoiceNumber).IsUnique();

            // Material stock unique (material_id, storage_location_id)
            b.Entity<MaterialStock>().HasIndex(x => new { x.MaterialId, x.StorageLocationId }).IsUnique();

            // Balances composite keys
            b.Entity<MaterialProjectBalance>().HasKey(x => new { x.MaterialId, x.ProjectId });
            b.Entity<MaterialVehicleBalance>().HasKey(x => new { x.MaterialId, x.VehicleId });

            // ToolAssetStock PK = FK (tool_asset_id)
            b.Entity<ToolAssetStock>().HasKey(x => x.ToolAssetId);

            // Favorites/Reads unique
            b.Entity<KnowledgeArticleFavorite>().HasIndex(x => new { x.ArticleId, x.UserId }).IsUnique();
            b.Entity<KnowledgeArticleRead>().HasIndex(x => new { x.ArticleId, x.UserId }).IsUnique();

            // Decimal precision (adjust if needed per DB)
            b.Entity<MaterialStock>().Property(x => x.Quantity).HasPrecision(18, 3);
            b.Entity<MaterialMovement>().Property(x => x.Quantity).HasPrecision(18, 3);
            b.Entity<MaterialProjectBalance>().Property(x => x.Quantity).HasPrecision(18, 3);
            b.Entity<MaterialVehicleBalance>().Property(x => x.Quantity).HasPrecision(18, 3);
            b.Entity<StockRequestItem>().Property(x => x.RequestedQuantity).HasPrecision(18, 3);
            b.Entity<StockRequestFulfillmentItem>().Property(x => x.Quantity).HasPrecision(18, 3);
            b.Entity<StockRequestReservation>().Property(x => x.ReservedQuantity).HasPrecision(18, 3);
            b.Entity<InvoiceItem>().Property(x => x.Quantity).HasPrecision(18, 3);
            b.Entity<InvoiceItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);
            b.Entity<InvoiceItem>().Property(x => x.LineSubtotal).HasPrecision(18, 2);
            b.Entity<InvoiceItem>().Property(x => x.LineTax).HasPrecision(18, 2);
            b.Entity<InvoiceItem>().Property(x => x.LineTotal).HasPrecision(18, 2);
            b.Entity<Invoice>().Property(x => x.SubtotalAmount).HasPrecision(18, 2);
            b.Entity<Invoice>().Property(x => x.TaxAmount).HasPrecision(18, 2);
            b.Entity<Invoice>().Property(x => x.TotalAmount).HasPrecision(18, 2);
            b.Entity<ServiceOrder>().Property(x => x.TotalCost).HasPrecision(18, 2);
            b.Entity<BillingRate>().Property(x => x.HourlyRate).HasPrecision(18, 2);
            b.Entity<MaterialPriceList>().Property(x => x.Price).HasPrecision(18, 2);

            // Check constraints
            b.Entity<EmployeeCompensation>()
                .HasCheckConstraint("ck_employee_comp_valid_to", "valid_to IS NULL OR valid_to >= valid_from");

            // Relationships (najważniejsze)
            b.Entity<Person>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<Employee>()
                .HasOne(x => x.Person)
                .WithMany()
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Employee>()
                .HasOne(x => x.Position)
                .WithMany()
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<Project>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<Project>()
                .HasOne(x => x.ResponsibleEmployee)
                .WithMany()
                .HasForeignKey(x => x.ResponsibleEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            b.Entity<ProjectAccess>()
                .HasOne(x => x.Project).WithMany(x => x.Accesses)
                .HasForeignKey(x => x.ProjectId);

            b.Entity<ProjectAccess>()
                .HasOne(x => x.Employee).WithMany()
                .HasForeignKey(x => x.EmployeeId);

            b.Entity<ProjectTask>()
                .HasOne(x => x.Project).WithMany(x => x.Tasks)
                .HasForeignKey(x => x.ProjectId);

            b.Entity<ProjectTask>()
                .HasOne(x => x.CreatedByUser).WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<ProjectTask>()
                .HasOne(x => x.AssignedToEmployee).WithMany()
                .HasForeignKey(x => x.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            b.Entity<WorkSession>()
                .HasOne(x => x.Employee).WithMany()
                .HasForeignKey(x => x.EmployeeId);

            b.Entity<WorkSession>()
                .HasOne(x => x.Project).WithMany()
                .HasForeignKey(x => x.ProjectId);

            b.Entity<WorkSession>()
                .HasOne(x => x.StartedByUser).WithMany()
                .HasForeignKey(x => x.StartedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<LeaveRequest>()
                .HasOne(x => x.Employee).WithMany()
                .HasForeignKey(x => x.EmployeeId);

            b.Entity<LeaveRequest>()
                .HasOne(x => x.RequestedByUser).WithMany()
                .HasForeignKey(x => x.RequestedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<LeaveRequest>()
                .HasOne(x => x.ApprovedByUser).WithMany()
                .HasForeignKey(x => x.ApprovedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            b.Entity<MaterialCategory>()
                .HasOne(x => x.Parent).WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<ToolCategory>()
                .HasOne(x => x.Parent).WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<KnowledgeCategory>()
                .HasOne(x => x.Parent).WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
