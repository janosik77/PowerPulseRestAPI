using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class ProjectTaskConfiguration
    {
        public void Configure(EntityTypeBuilder<ProjectTask> b)
        {
            b.ToTable("project_tasks");

            // PK
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            // project
            b.Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            // title
            b.Property(x => x.Title)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(300);

            // description
            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(4000);

            // enums
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

            // due date
            b.Property(x => x.DueAt)
                .HasColumnName("due_at");

            // estimate
            b.Property(x => x.EstimatedMinutes)
                .HasColumnName("estimated_minutes");

            // creator
            b.Property(x => x.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired();

            // assigned
            b.Property(x => x.AssignedToEmployeeId)
                .HasColumnName("assigned_to_employee_id");

            // timestamps
            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            // RELACJE

            b.HasOne(x => x.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.AssignedToEmployee)
                .WithMany(e => e.AssignedTasks)
                .HasForeignKey(x => x.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            // INDEKSY

            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.AssignedToEmployeeId);
            b.HasIndex(x => x.DueAt);

            // bardzo częste zapytanie: taski projektu wg statusu
            b.HasIndex(x => new { x.ProjectId, x.Status });

            // CONSTRAINTY

            b.HasCheckConstraint(
                "ck_project_task_estimate_positive",
                "estimated_minutes IS NULL OR estimated_minutes >= 0"
            );
        }
    }
}
