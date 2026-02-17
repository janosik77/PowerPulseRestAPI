using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class TaskUpdateConfiguration
    {
        public void Configure(EntityTypeBuilder<TaskUpdate> b)
        {
            b.ToTable("task_updates");

            // PK
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            // FK -> task
            b.Property(x => x.TaskId)
                .HasColumnName("task_id")
                .IsRequired();

            // enum
            b.Property(x => x.UpdateType)
                .HasColumnName("update_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            // content
            b.Property(x => x.Content)
                .HasColumnName("content")
                .IsRequired()
                .HasMaxLength(8000);

            // FK -> user
            b.Property(x => x.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired();

            // timestamp
            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            // RELACJE

            // ProjectTask 1 -> N TaskUpdates
            b.HasOne(x => x.Task)
                .WithMany(t => t.Updates) 
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // User 1 -> N TaskUpdates (autor)
            b.HasOne(x => x.CreatedByUser)
                .WithMany(u => u.TaskUpdates) 
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // INDEKSY

            // najczęstsze: historia update'ów dla danego taska po czasie
            b.HasIndex(x => new { x.TaskId, x.CreatedAt });

            // czasem: co robił user
            b.HasIndex(x => x.CreatedByUserId);
        }
    }
}
