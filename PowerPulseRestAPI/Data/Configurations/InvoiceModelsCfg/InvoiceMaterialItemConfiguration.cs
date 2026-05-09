using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.InvoiceModels;

namespace PowerPulseRestAPI.Data.Configurations.MaterialsConfigurations
{
    public class InvoiceMaterialItemConfiguration : IEntityTypeConfiguration<InvoiceMaterialItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceMaterialItem> b)
        {
            b.ToTable("invoice_material_items");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.InvoiceId)
                .HasColumnName("invoice_id")
                .IsRequired();

            b.Property(x => x.MaterialId)
                .HasColumnName("material_id")
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

            b.HasIndex(x => x.InvoiceId);

            b.HasIndex(x => x.MaterialId);

            b.HasIndex(x => new { x.InvoiceId, x.MaterialId });

            b.HasOne(x => x.Invoice)
                .WithMany(x => x.MaterialItems)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Material)
                .WithMany(x => x.InvoiceMaterialItems)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}