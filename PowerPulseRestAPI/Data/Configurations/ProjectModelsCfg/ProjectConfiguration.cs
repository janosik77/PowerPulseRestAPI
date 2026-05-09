using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations.ProjectModelsCfg
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> b)
        {
            b.ToTable("projects");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();

            b.Property(x => x.Code)
                .HasColumnName("code")
                .IsRequired()
                .HasMaxLength(50);

            b.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);

            b.Property(x => x.AvatarUrl)
            .HasColumnName("avatar_url")
            .HasMaxLength(2048);

            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(4000);

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("date");

            b.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .HasColumnType("date");

            b.Property(x => x.AddressId)
                .HasColumnName("address_id")
                .IsRequired();

            b.Property(x => x.CreatedByEmployeeId)
                .HasColumnName("created_by_employee_id")
                .IsRequired();

            b.Property(x => x.ResponsibleEmployeeId)
                .HasColumnName("responsible_employee_id");

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired()
                .HasDefaultValue(false);

            b.HasOne(x => x.Customer)
                .WithMany(c => c.Projects)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.CreatedByEmployee)
                .WithMany()
                .HasForeignKey(x => x.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.ResponsibleEmployee)
                .WithMany()
                .HasForeignKey(x => x.ResponsibleEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            b.HasOne(x => x.Address)
                .WithOne()
                .HasForeignKey<Project>(x => x.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.Code)
                .IsUnique();
            b.HasIndex(x => x.Name)
                .IsUnique();

            b.HasIndex(x => x.CustomerId);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.CreatedByEmployeeId);
            b.HasIndex(x => x.ResponsibleEmployeeId);
            b.HasIndex(x => x.StartDate);
            b.HasIndex(x => x.EndDate);
            b.HasIndex(x => x.Name);
            b.HasIndex(x => x.AddressId).IsUnique();

            b.HasCheckConstraint(
                "ck_project_dates",
                "end_date IS NULL OR start_date IS NULL OR end_date >= start_date"
            );

            b.HasCheckConstraint("ck_project_code_not_empty", "code <> ''");
            b.HasCheckConstraint("ck_project_name_not_empty", "name <> ''");
        }
    }
}