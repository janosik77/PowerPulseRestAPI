using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Configurations.UserModelsCfg;

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> b)
    {
        b.ToTable("activity_logs");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.EntityType)
            .HasColumnName("entity_type")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.EntityId)
            .HasColumnName("entity_id")
            .IsRequired();

        b.Property(x => x.ActionType)
            .HasColumnName("action_type")
            .HasConversion<string>()
            .HasMaxLength(40)
            .IsRequired();

        b.Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(2000);

        b.Property(x => x.CreatedByUserId)
            .HasColumnName("created_by_user_id")
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.HasOne(x => x.CreatedByUser)
            .WithMany(u => u.ActivityLogs)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indeksy – typowe: pobierz historię encji / akcje usera
        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => x.CreatedByUserId);
        b.HasIndex(x => new { x.EntityType, x.EntityId, x.CreatedAt });
        b.HasIndex(x => x.ActionType);

        b.HasCheckConstraint("ck_activity_log_entity_id_positive", "entity_id > 0");
    }
}
