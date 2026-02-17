using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class ProjectConfiguration
    {
        public void Configure(EntityTypeBuilder<Project> b)
        {
            b.ToTable("projects");

            // PK
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            // CODE
            b.Property(x => x.Code)
                .HasColumnName("code")
                .IsRequired()
                .HasMaxLength(50);

            b.HasIndex(x => x.Code)
                .IsUnique();

            // NAME
            b.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);

            // DESCRIPTION
            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(4000);

            // STATUS
            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            // DATES
            b.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("date");

            b.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .HasColumnType("date");

            // CREATED BY USER
            b.Property(x => x.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired();

            // RESPONSIBLE EMPLOYEE
            b.Property(x => x.ResponsibleEmployeeId)
                .HasColumnName("responsible_employee_id");

            // TIMESTAMPS
            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            // RELACJE

            // creator user
            b.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // responsible employee
            b.HasOne(x => x.ResponsibleEmployee)
                .WithMany()
                .HasForeignKey(x => x.ResponsibleEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            // project accesses
            b.HasMany(x => x.Accesses)
                .WithOne()
                .HasForeignKey("project_id")
                .OnDelete(DeleteBehavior.Cascade);

            // project tasks
            b.HasMany(x => x.Tasks)
                .WithOne()
                .HasForeignKey("project_id")
                .OnDelete(DeleteBehavior.Cascade);

            // INDEKSY

            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.CreatedByUserId);
            b.HasIndex(x => x.ResponsibleEmployeeId);
            b.HasIndex(x => x.StartDate);

            // CONSTRAINTY

            b.HasCheckConstraint(
                "ck_project_dates",
                "end_date IS NULL OR start_date IS NULL OR end_date >= start_date"
            );
        }
    }
}
