using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.WorkSessionModels;

namespace PowerPulseRestAPI.Data.Configurations.UserModelsCfg
{
    public class WorkSessionConfiguration : IEntityTypeConfiguration<WorkSession>
    {
        public void Configure(EntityTypeBuilder<WorkSession> b)
        {
            b.ToTable("work_sessions");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired();

            b.Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            b.Property(x => x.StartedAt)
                .HasColumnName("started_at")
                .IsRequired();

            b.Property(x => x.EndedAt)
                .HasColumnName("ended_at");

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.Note)
                .HasColumnName("note")
                .HasMaxLength(2000);

            b.Property(x => x.InvoiceId)
                .HasColumnName("invoice_id");

            b.Property(x => x.InvoicedAt)
                .HasColumnName("invoiced_at");

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.HasOne(x => x.Employee)
                .WithMany(e => e.WorkSessions)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Project)
                .WithMany(p => p.WorkSessions)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Invoice)
                .WithMany(i => i.WorkSessions)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.EmployeeId);
            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.StartedAt);
            b.HasIndex(x => x.InvoiceId);
            b.HasIndex(x => x.InvoicedAt);

            b.HasIndex(x => new { x.EmployeeId, x.ProjectId, x.StartedAt });

            b.HasCheckConstraint(
                "ck_work_session_end_after_start",
                "ended_at IS NULL OR ended_at >= started_at"
            );
        }
    }
}
