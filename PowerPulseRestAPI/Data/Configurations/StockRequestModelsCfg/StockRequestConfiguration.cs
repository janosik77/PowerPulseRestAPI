using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

public class StockRequestConfiguration : IEntityTypeConfiguration<StockRequest>
{
    public void Configure(EntityTypeBuilder<StockRequest> b)
    {
        b.ToTable("stock_requests");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .HasColumnName("id");

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

        b.Property(x => x.ProjectId)
            .HasColumnName("project_id");

        b.Property(x => x.RequestedByUserId)
            .HasColumnName("requested_by_user_id")
            .IsRequired();

        b.Property(x => x.RequestedAt)
            .HasColumnName("requested_at")
            .IsRequired();

        b.Property(x => x.ApprovedByUserId)
            .HasColumnName("approved_by_user_id");

        b.Property(x => x.ApprovedAt)
            .HasColumnName("approved_at");

        b.Property(x => x.ApprovalNote)
            .HasColumnName("approval_note")
            .HasMaxLength(2000);

        b.Property(x => x.Note)
            .HasColumnName("note")
            .HasMaxLength(2000);

        b.Property(x => x.ReceivedConfirmedByUserId)
            .HasColumnName("received_confirmed_by_user_id");

        b.Property(x => x.ReceivedConfirmedAt)
            .HasColumnName("received_confirmed_at");

        b.Property(x => x.ReceivedConfirmationNote)
            .HasColumnName("received_confirmation_note")
            .HasMaxLength(2000);

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        b.HasOne(x => x.Project)
            .WithMany(p => p.StockRequests)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.RequestedByUser)
            .WithMany(u => u.StockRequestsRequested)
            .HasForeignKey(x => x.RequestedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ApprovedByUser)
            .WithMany(u => u.StockRequestsApproved)
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ReceivedConfirmedByUser)
            .WithMany(u => u.StockRequestsReceivedConfirmed)
            .HasForeignKey(x => x.ReceivedConfirmedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(x => x.Items)
                .WithOne(x => x.StockRequest)
                .HasForeignKey(x => x.StockRequestId)
                .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(x => x.History)
            .WithOne(x => x.StockRequest)
            .HasForeignKey(x => x.StockRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.ProjectId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.Priority);
        b.HasIndex(x => x.RequestType);
        b.HasIndex(x => x.RequestedByUserId);
        b.HasIndex(x => x.ApprovedByUserId);
        b.HasIndex(x => x.ReceivedConfirmedByUserId);
        b.HasIndex(x => x.RequestedAt);
        b.HasIndex(x => new { x.ProjectId, x.Status });
        b.HasIndex(x => new { x.RequestedByUserId, x.RequestedAt });

        b.HasCheckConstraint(
            "ck_stock_request_approval_consistency",
            "(approved_at IS NULL AND approved_by_user_id IS NULL) OR (approved_at IS NOT NULL AND approved_by_user_id IS NOT NULL)"
        );

        b.HasCheckConstraint(
            "ck_stock_request_received_confirmation_consistency",
            "(received_confirmed_at IS NULL AND received_confirmed_by_user_id IS NULL) OR (received_confirmed_at IS NOT NULL AND received_confirmed_by_user_id IS NOT NULL)"
        );
        b.HasCheckConstraint(
                "ck_stock_request_updated_at",
                "updated_at >= created_at"
            );
    }
}
