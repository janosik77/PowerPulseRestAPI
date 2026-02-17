using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class TaskAttachmentConfiguration
    {
        public void Configure(EntityTypeBuilder<TaskAttachment> b)
        {
            b.ToTable("task_attachments");

            b.HasKey(x => x.Id);

            b.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(2048);

            b.Property(x => x.Caption)
                .HasMaxLength(500);

            b.Property(x => x.CreatedAt)
                .IsRequired();

            // Indeksy typowe dla listowania załączników dla zadania
            b.HasIndex(x => x.TaskId);
            b.HasIndex(x => new { x.TaskId, x.CreatedAt });

            // FK: TaskAttachment -> ProjectTask
            // Zwykle: usunięcie zadania usuwa załączniki (Cascade)
            b.HasOne(x => x.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
