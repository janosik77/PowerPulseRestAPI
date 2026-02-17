using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class EmployeeConfiguration
    {
        public void Configure(EntityTypeBuilder<Employee> b)
        {
            b.ToTable("employee");

            // PK
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            // FK -> person (1:1)
            b.Property(x => x.PersonId)
                .HasColumnName("person_id")
                .IsRequired();

            // DateOnly -> date
            b.Property(x => x.HireDate)
                .HasColumnName("hire_date")
                .HasColumnType("date")
                .IsRequired();

            b.Property(x => x.TerminatedAt)
                .HasColumnName("terminated_at")
                .HasColumnType("date");

            // FK -> positions
            b.Property(x => x.PositionId)
                .HasColumnName("position_id")
                .IsRequired();

            b.Property(x => x.Department)
                .HasColumnName("department")
                .HasMaxLength(100);

            // Enums jako string (czytelnie i bezpiecznie)
            b.Property(x => x.EmployeeType)
                .HasColumnName("employee_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.RemainingVacationDays)
                .HasColumnName("remaining_vacation_days")
                .IsRequired();

            b.Property(x => x.VacationDaysPerYear)
                .HasColumnName("vacation_days_per_year")
                .IsRequired();

            // 1:1 Person <-> Employee (Employee zależny od Person)
            b.HasIndex(x => x.PersonId).IsUnique();

            b.HasOne(x => x.Person)
                .WithOne(p => p.Employee) // jeśli dodasz Person.Employee, zmień na .WithOne(p => p.Employee)
                .HasForeignKey<Employee>(x => x.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // N:1 Position
            b.HasOne(x => x.Position)
                .WithMany()
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            // (opcjonalnie) indeksy pod typowe filtry
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.PositionId);

            // (opcjonalnie) podstawowe constrainty biznesowe
            b.HasCheckConstraint("ck_employee_vacation_nonneg", "remaining_vacation_days >= 0 AND vacation_days_per_year >= 0");
            b.HasCheckConstraint("ck_employee_terminated_after_hire", "terminated_at IS NULL OR terminated_at >= hire_date");
        }
    }
}
