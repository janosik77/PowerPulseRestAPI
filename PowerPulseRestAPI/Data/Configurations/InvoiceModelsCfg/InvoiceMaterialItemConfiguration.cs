using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.InvoiceModels;

namespace PowerPulseRestAPI.Data.Configurations.MaterialsConfigurations
{
    public class InvoiceMaterialItemConfiguration : IEntityTypeConfiguration<InvoiceMaterialItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceMaterialItem> builder)
        {
            builder.ToTable("InvoiceMaterialItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Unit)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.UnitPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.TaxRate)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(x => x.LineSubtotal)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.LineTax)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.LineTotal)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.InvoiceId);

            builder.HasIndex(x => x.MaterialId);

            builder.HasIndex(x => new { x.InvoiceId, x.MaterialId });

            builder.HasOne(x => x.Invoice)
                .WithMany(x => x.MaterialItems)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Material)
                .WithMany(x => x.InvoiceMaterialItems)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}