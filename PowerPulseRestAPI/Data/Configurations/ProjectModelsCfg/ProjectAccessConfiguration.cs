using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.ProjectModels;


namespace PowerPulseRestAPI.Data.Configurations.ProjectModelsCfg
{
    public class ProjectAccessConfiguration : IEntityTypeConfiguration<ProjectAccess>
    {
        public void Configure(EntityTypeBuilder<ProjectAccess> b)
        {
            b.ToTable("project_access");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            b.Property(x => x.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired();

            b.Property(x => x.IsEnabled)
                .HasColumnName("is_enabled")
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.HasOne(x => x.Project)
                .WithMany(p => p.Accesses)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Employee)
                .WithMany(e => e.ProjectAccesses)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => x.EmployeeId);

            b.HasIndex(x => new { x.ProjectId, x.EmployeeId }).IsUnique();

        }
    }
}
