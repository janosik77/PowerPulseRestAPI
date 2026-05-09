using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.InvoiceModels;

namespace PowerPulseRestAPI.Data.Configurations.InvoiceConfigurations
{
    public class InvoiceLaborItemConfiguration : IEntityTypeConfiguration<InvoiceLaborItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceLaborItem> b)
        {
            b.ToTable("invoice_labor_items");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.InvoiceId)
                .HasColumnName("invoice_id")
                .IsRequired();

            b.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .HasPrecision(18, 2)
                .IsRequired();

            b.Property(x => x.Unit)
                .HasColumnName("unit")
                .HasMaxLength(20)
                .IsRequired();

            b.Property(x => x.UnitPrice)
                .HasColumnName("unit_price")
                .HasPrecision(18, 2)
                .IsRequired();

            b.Property(x => x.TaxRate)
                .HasColumnName("tax_rate")
                .HasPrecision(5, 2)
                .IsRequired();

            b.Property(x => x.LineSubtotal)
                .HasColumnName("line_subtotal")
                .HasPrecision(18, 2)
                .IsRequired();

            b.Property(x => x.LineTax)
                .HasColumnName("line_tax")
                .HasPrecision(18, 2)
                .IsRequired();

            b.Property(x => x.LineTotal)
                .HasColumnName("line_total")
                .HasPrecision(18, 2)
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.HasIndex(x => x.InvoiceId)
                .IsUnique();

            b.HasOne(x => x.Invoice)
                .WithOne(x => x.LaborItem)
                .HasForeignKey<InvoiceLaborItem>(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}