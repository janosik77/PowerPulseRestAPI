using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.InvoiceModels;

namespace PowerPulseRestAPI.Data.Configurations.InvoiceConfigurations
{
    public class InvoiceLaborItemConfiguration : IEntityTypeConfiguration<InvoiceLaborItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceLaborItem> builder)
        {
            builder.ToTable("InvoiceLaborItems");

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

            builder.HasIndex(x => x.InvoiceId)
                .IsUnique();

            builder.HasOne(x => x.Invoice)
                .WithOne(x => x.LaborItem)
                .HasForeignKey<InvoiceLaborItem>(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}