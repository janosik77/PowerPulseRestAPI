using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.InvoiceModels;

public class BillingRateConfiguration : IEntityTypeConfiguration<BillingRate>
{
    public void Configure(EntityTypeBuilder<BillingRate> b)
    {
        b.ToTable("billing_rates");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ProjectId)
            .HasColumnName("project_id")
            .IsRequired();

        b.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        b.Property(x => x.HourlyRate)
            .HasColumnName("hourly_rate")
            .HasPrecision(18, 2)
            .IsRequired();

        b.Property(x => x.Currency)
            .HasColumnName("currency")
            .HasMaxLength(10)
            .IsRequired();

        b.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        b.Property(x => x.ValidFrom)
            .HasColumnName("valid_from")
            .HasColumnType("date")
            .IsRequired();

        b.Property(x => x.ValidTo)
            .HasColumnName("valid_to")
            .HasColumnType("date");

        // Relacja: Project 1 -> * BillingRates (dwukierunkowo)
        b.HasOne(x => x.Project)
            .WithMany(p => p.BillingRates)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indeksy (wydajność)
        b.HasIndex(x => x.ProjectId);
        b.HasIndex(x => x.IsActive);
        b.HasIndex(x => x.ValidFrom);
        b.HasIndex(x => x.ValidTo);

        // Najważniejszy indeks pod pobranie stawki dla projektu na daną datę
        b.HasIndex(x => new { x.ProjectId, x.IsActive, x.ValidFrom, x.ValidTo });

        // CHECK constrainty "wierszowe" (bez porównywania do innych rekordów)
        b.HasCheckConstraint(
            "ck_billing_rate_valid_range",
            "valid_to IS NULL OR valid_to >= valid_from"
        );

        b.HasCheckConstraint(
            "ck_billing_rate_hourly_nonneg",
            "hourly_rate >= 0"
        );

        b.HasCheckConstraint(
            "ck_billing_rate_currency_not_empty",
            "currency <> ''"
        );

        b.HasCheckConstraint(
            "ck_billing_rate_name_not_empty",
            "name <> ''"
        );
    }
}
