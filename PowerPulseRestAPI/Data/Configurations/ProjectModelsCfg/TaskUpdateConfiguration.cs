using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations.ProjectModelsCfg
{
    public class TaskUpdateConfiguration : IEntityTypeConfiguration<TaskUpdate>
    {
        public void Configure(EntityTypeBuilder<TaskUpdate> b)
        {
            b.ToTable("task_updates");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.TaskId)
                .HasColumnName("task_id")
                .IsRequired();

            b.Property(x => x.UpdateType)
                .HasColumnName("update_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.Content)
                .HasColumnName("content")
                .IsRequired()
                .HasMaxLength(8000);

            b.Property(x => x.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.HasOne(x => x.Task)
                .WithMany(t => t.Updates)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.CreatedByUser)
                .WithMany(u => u.TaskUpdates)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => new { x.TaskId, x.CreatedAt });
            b.HasIndex(x => x.CreatedByUserId);

            b.HasCheckConstraint("ck_task_update_content_not_empty", "content <> ''");
        }
    }
}