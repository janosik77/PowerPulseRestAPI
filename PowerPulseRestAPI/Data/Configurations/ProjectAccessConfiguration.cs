using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class ProjectAccessConfiguration
    {
        public void Configure(EntityTypeBuilder<ProjectAccess> b)
        {
            b.ToTable("project_access");

            // PK
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            // FKs
            b.Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            b.Property(x => x.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired();

            // dates
            b.Property(x => x.ValidFrom)
                .HasColumnName("valid_from")
                .HasColumnType("date");

            b.Property(x => x.ValidTo)
                .HasColumnName("valid_to")
                .HasColumnType("date");

            // enabled
            b.Property(x => x.IsEnabled)
                .HasColumnName("is_enabled")
                .IsRequired();

            // timestamp
            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            // RELACJE
            b.HasOne(x => x.Project)
                .WithMany(p => p.Accesses) 
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict); 

            b.HasOne(x => x.Employee)
                .WithMany(e => e.ProjectAccesses)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // INDEKSY (pod realne zapytania)
            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => x.EmployeeId);

            // Najczęstsze sprawdzenie: "czy employee ma dostęp do projektu?"
            b.HasIndex(x => new { x.ProjectId, x.EmployeeId });

            // (opcjonalnie) jeśli często filtrujesz tylko aktywne dostępy
            b.HasIndex(x => new { x.ProjectId, x.IsEnabled });

            // CONSTRAINTY
            b.HasCheckConstraint(
                "ck_project_access_dates",
                "valid_to IS NULL OR valid_from IS NULL OR valid_to >= valid_from"
            );

            b.HasIndex(x => new { x.ProjectId, x.EmployeeId })
                .IsUnique();
        }
    }
}
