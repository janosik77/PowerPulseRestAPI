using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Configurations.EmployeeModelsCfg
{
    public class EmployeeCompensationConfiguration : IEntityTypeConfiguration<EmployeeCompensation>
    {
        public void Configure(EntityTypeBuilder<EmployeeCompensation> b)
        {
            b.ToTable("employee_compensations");

            // PK
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired();

            b.Property(x => x.HourlyWageEncrypted)
                .HasColumnName("hourly_wage_encrypted")
                .IsRequired()
                .HasMaxLength(500);

            b.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(10)
                .IsRequired();

            b.Property(x => x.ValidFrom)
                .HasColumnName("valid_from")
                .HasColumnType("date")
                .IsRequired();

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

            b.HasIndex(x => x.EmployeeId);
            b.HasIndex(x => new { x.EmployeeId, x.ValidFrom });

        }
    }
}
