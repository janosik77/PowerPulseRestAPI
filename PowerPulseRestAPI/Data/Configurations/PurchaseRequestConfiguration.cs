using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.PurchaseReqModels;

public class PurchaseRequestConfiguration : IEntityTypeConfiguration<PurchaseRequest>
{
    public void Configure(EntityTypeBuilder<PurchaseRequest> b)
    {
        b.ToTable("purchase_requests");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ProjectId).HasColumnName("project_id").IsRequired();
        b.Property(x => x.RequestedByUserId).HasColumnName("requested_by_user_id").IsRequired();

        b.Property(x => x.Priority)
            .HasColumnName("priority")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.Note)
            .HasColumnName("note")
            .HasMaxLength(2000);

        b.Property(x => x.ApprovedByUserId).HasColumnName("approved_by_user_id");
        b.Property(x => x.ApprovedAt).HasColumnName("approved_at");

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        // Relacje (dwukierunkowe)
        b.HasOne(x => x.Project)
            .WithMany(p => p.PurchaseRequests)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.RequestedByUser)
            .WithMany(u => u.PurchaseRequestsCreated)
            .HasForeignKey(x => x.RequestedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ApprovedByUser)
            .WithMany(u => u.PurchaseRequestsApproved)
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indeksy
        b.HasIndex(x => x.ProjectId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.Priority);
        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => new { x.ProjectId, x.Status });

        // Constraint: jeśli ApprovedAt jest ustawione, ApprovedByUserId też musi być
        b.HasCheckConstraint(
            "ck_purchase_request_approval_consistency",
            "(approved_at IS NULL AND approved_by_user_id IS NULL) OR (approved_at IS NOT NULL AND approved_by_user_id IS NOT NULL)"
        );
    }
}
