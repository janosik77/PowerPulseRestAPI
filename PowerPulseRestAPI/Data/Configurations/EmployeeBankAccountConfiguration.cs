using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class EmployeeBankAccountConfiguration
    {
        public void Configure(EntityTypeBuilder<EmployeeBankAccount> b)
        {
            b.ToTable("employee_bank_accounts");

            // PK
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            // FK
            b.Property(x => x.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired();

            // encrypted account
            b.Property(x => x.AccountEncrypted)
                .HasColumnName("account_encrypted")
                .IsRequired()
                .HasMaxLength(500);

            // last4
            b.Property(x => x.AccountLast4)
                .HasColumnName("account_last4")
                .HasMaxLength(10);

            // country
            b.Property(x => x.Country)
                .HasColumnName("country")
                .HasMaxLength(10);

            // primary flag
            b.Property(x => x.IsPrimary)
                .HasColumnName("is_primary")
                .IsRequired();

            // dates
            b.Property(x => x.ValidFrom)
                .HasColumnName("valid_from")
                .HasColumnType("date");

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

            // relacja Employee 1 -> N Accounts
            b.HasOne(x => x.Employee)
                .WithMany(e => e.BankAccounts)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // indeksy
            b.HasIndex(x => x.EmployeeId);
            b.HasIndex(x => new { x.EmployeeId, x.IsPrimary });

            // constraint: data końca >= start
            b.HasCheckConstraint(
                "ck_employee_bank_account_dates",
                "valid_to IS NULL OR valid_to >= valid_from"
            );
        }
    }
}
