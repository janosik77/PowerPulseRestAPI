using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.InvoiceModels;

public class InvoiceItemSourceConfiguration : IEntityTypeConfiguration<InvoiceItemSource>
{
    public void Configure(EntityTypeBuilder<InvoiceItemSource> b)
    {
        b.ToTable("invoice_item_sources");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.InvoiceItemId).HasColumnName("invoice_item_id").IsRequired();

        b.Property(x => x.SourceType)
            .HasColumnName("source_type")
            .HasConversion<string>()
            .HasMaxLength(40)
            .IsRequired();

        b.Property(x => x.SourceId).HasColumnName("source_id").IsRequired();

        b.Property(x => x.Quantity).HasColumnName("quantity").HasPrecision(18, 3).IsRequired();
        b.Property(x => x.Unit).HasColumnName("unit").IsRequired().HasMaxLength(20);
        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasOne(x => x.InvoiceItem)
            .WithMany(ii => ii.Sources)
            .HasForeignKey(x => x.InvoiceItemId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.InvoiceItemId);
        b.HasIndex(x => new { x.SourceType, x.SourceId });

        b.HasCheckConstraint("ck_invoice_item_source_qty_positive", "quantity > 0");
        b.HasCheckConstraint("ck_invoice_item_source_source_id_positive", "source_id > 0");
    }
}
