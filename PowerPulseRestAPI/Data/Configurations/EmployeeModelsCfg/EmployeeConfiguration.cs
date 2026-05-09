using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Configurations.EmployeeModelsCfg
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> b)
        {
            b.ToTable("employees");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.PersonId)
                .HasColumnName("person_id")
                .IsRequired();

            b.Property(x => x.HireDate)
                .HasColumnName("hire_date")
                .HasColumnType("date")
                .IsRequired();

            b.Property(x => x.TerminatedAt)
                .HasColumnName("terminated_at")
                .HasColumnType("date");

            b.Property(x => x.Department)
                .HasColumnName("department")
                .HasMaxLength(100);

            b.Property(x => x.JobTitle)
                .HasColumnName("job_title")
                .IsRequired()
                .HasMaxLength(150);

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.HourlyWage)
                .HasColumnName("hourly_wage")
                .HasPrecision(18, 2)
                .IsRequired();

            b.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(10)
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.Property(x => x.RemainingVacationDays)
                .HasColumnName("remaining_vacation_days")
                .IsRequired();

            b.Property(x => x.VacationDaysPerYear)
                .HasColumnName("vacation_days_per_year")
                .IsRequired();

            b.Property(x => x.AccountEncrypted)
                .HasColumnName("account_encrypted")
                .IsRequired()
                .HasMaxLength(500);

            b.Property(x => x.AccountLast4)
                .HasColumnName("account_last4")
                .HasMaxLength(10);

            b.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired()
                .HasDefaultValue(false);

            b.HasIndex(x => x.PersonId).IsUnique();
            b.HasIndex(x => x.Status);

            b.HasOne(x => x.Person)
                .WithOne(p => p.Employee)
                .HasForeignKey<Employee>(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasCheckConstraint(
                "ck_employee_vacation_nonneg",
                "remaining_vacation_days >= 0 AND vacation_days_per_year >= 0");

            b.HasCheckConstraint(
                "ck_employee_terminated_after_hire",
                "terminated_at IS NULL OR terminated_at >= hire_date");

            b.HasCheckConstraint(
                "ck_employee_job_title_not_empty",
                "job_title <> ''");
        }
    }
}