using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class EmployeeCompensationConfiguration
    {
        public void Configure(EntityTypeBuilder<EmployeeCompensation> b)
        {
            b.ToTable("employee_compensations");

            // PK
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            // FK
            b.Property(x => x.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired();

            // encrypted hourly wage
            b.Property(x => x.HourlyWageEncrypted)
                .HasColumnName("hourly_wage_encrypted")
                .IsRequired()
                .HasMaxLength(500);

            // encrypted pension
            b.Property(x => x.ExtraPensionEncrypted)
                .HasColumnName("extra_pension_encrypted")
                .HasMaxLength(500);

            // currency
            b.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(10)
                .IsRequired();

            // dates
            b.Property(x => x.ValidFrom)
                .HasColumnName("valid_from")
                .HasColumnType("date")
                .IsRequired();

            b.Property(x => x.ValidTo)
                .HasColumnName("valid_to")
                .HasColumnType("date");

            // timestamps
            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            // relacja Employee 1 -> N Compensation
            b.HasOne(x => x.Employee)
                .WithMany(e => e.Compensations)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // indeksy
            b.HasIndex(x => x.EmployeeId);
            b.HasIndex(x => new { x.EmployeeId, x.ValidFrom });

            // constraint biznesowy
            b.HasCheckConstraint(
                "ck_employee_compensation_dates",
                "valid_to IS NULL OR valid_to >= valid_from"
            );
        }
    }
}
