using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

public class StockRequestConfiguration : IEntityTypeConfiguration<StockRequest>
{
    public void Configure(EntityTypeBuilder<StockRequest> b)
    {
        b.ToTable("stock_requests");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.RequestType)
            .HasColumnName("request_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.Priority)
            .HasColumnName("priority")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.ProjectId).HasColumnName("project_id");
        b.Property(x => x.VehicleId).HasColumnName("vehicle_id");
        b.Property(x => x.EmployeeId).HasColumnName("employee_id");

        b.Property(x => x.RequestedByUserId).HasColumnName("requested_by_user_id").IsRequired();
        b.Property(x => x.RequestedAt).HasColumnName("requested_at").IsRequired();

        b.Property(x => x.ApprovedByUserId).HasColumnName("approved_by_user_id");
        b.Property(x => x.ApprovedAt).HasColumnName("approved_at");

        b.Property(x => x.ApprovalNote).HasColumnName("approval_note").HasMaxLength(2000);
        b.Property(x => x.Note).HasColumnName("note").HasMaxLength(2000);

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        // relacje (dwukierunkowe)
        b.HasOne(x => x.Project)
            .WithMany(p => p.StockRequests)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Vehicle)
            .WithMany(v => v.StockRequests)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Employee)
            .WithMany(e => e.StockRequests)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.RequestedByUser)
            .WithMany(u => u.StockRequestsRequested)
            .HasForeignKey(x => x.RequestedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ApprovedByUser)
            .WithMany(u => u.StockRequestsApproved)
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // indeksy
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.Priority);
        b.HasIndex(x => x.RequestedAt);
        b.HasIndex(x => x.RequestedByUserId);
        b.HasIndex(x => x.ProjectId);
        b.HasIndex(x => x.VehicleId);
        b.HasIndex(x => x.EmployeeId);

        // constraint: dokładnie jedno z project/vehicle/employee ustawione
        b.HasCheckConstraint(
            "ck_stock_request_single_scope",
            "(" +
            " (CASE WHEN project_id IS NULL THEN 0 ELSE 1 END) + " +
            " (CASE WHEN vehicle_id IS NULL THEN 0 ELSE 1 END) + " +
            " (CASE WHEN employee_id IS NULL THEN 0 ELSE 1 END) " +
            ") = 1"
        );

        // constraint: spójność approval
        b.HasCheckConstraint(
            "ck_stock_request_approval_consistency",
            "(approved_at IS NULL AND approved_by_user_id IS NULL) OR (approved_at IS NOT NULL AND approved_by_user_id IS NOT NULL)"
        );
    }
}
