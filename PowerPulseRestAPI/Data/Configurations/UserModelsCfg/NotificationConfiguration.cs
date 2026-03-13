using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Configurations.UserModelsCfg;
public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> b)
    {
        b.ToTable("notifications");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .HasColumnName("id");

        b.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        b.Property(x => x.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(300);

        b.Property(x => x.Content)
            .HasColumnName("content")
            .IsRequired()
            .HasMaxLength(4000);

        b.Property(x => x.Severity)
            .HasColumnName("severity")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.IsRead)
            .HasColumnName("is_read")
            .IsRequired();

        b.Property(x => x.ReadAt)
            .HasColumnName("read_at");

        b.Property(x => x.RelatedProjectId)
            .HasColumnName("related_project_id");

        b.Property(x => x.RelatedTaskId)
            .HasColumnName("related_task_id");

        b.Property(x => x.RelatedStockRequestId)
                .HasColumnName("related_stock_request_id");

        b.Property(x => x.RelatedToolId)
                .HasColumnName("related_tool_id");

        b.Property(x => x.RelatedVehicleId)
            .HasColumnName("related_vehicle_id");


        b.Property(x => x.CreatedByUserId)
            .HasColumnName("created_by_user_id");

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        // Relacje (dwukierunkowe dla User; pozostałe opcjonalnie)
        b.HasOne(x => x.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.CreatedByUser)
            .WithMany(u => u.CreatedNotifications)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.RelatedProject)
            .WithMany(p => p.Notifications)
            .HasForeignKey(x => x.RelatedProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.RelatedTask)
            .WithMany(t => t.Notifications)
            .HasForeignKey(x => x.RelatedTaskId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.RelatedStockRequest)
                .WithMany()
                .HasForeignKey(x => x.RelatedStockRequestId)
                .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.RelatedVehicle)
            .WithMany(v => v.Notifications)
            .HasForeignKey(x => x.RelatedVehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.RelatedTool)
                .WithMany()
                .HasForeignKey(x => x.RelatedToolId)
                .OnDelete(DeleteBehavior.Restrict);

        // Indeksy (wydajność listy notyfikacji)
        b.HasIndex(x => x.UserId);
        b.HasIndex(x => new { x.UserId, x.IsRead, x.CreatedAt });
        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => x.Type);
        b.HasIndex(x => x.Severity);

        b.HasIndex(x => x.RelatedProjectId);
        b.HasIndex(x => x.RelatedTaskId);
        b.HasIndex(x => x.RelatedVehicleId);
        b.HasIndex(x => x.RelatedToolId);
        b.HasIndex(x => x.RelatedStockRequestId);

        // CONSTRAINT: spójność IsRead i ReadAt
        b.HasCheckConstraint(
            "ck_notification_read_consistency",
            "(is_read = 0 AND read_at IS NULL) OR (is_read = 1 AND read_at IS NOT NULL)"
        );
    }
}
