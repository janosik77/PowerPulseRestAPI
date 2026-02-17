using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.InvoiceModels;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> b)
    {
        b.ToTable("invoice_items");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.InvoiceId).HasColumnName("invoice_id").IsRequired();

        b.Property(x => x.ItemType)
            .HasColumnName("item_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.Title).HasColumnName("title").IsRequired().HasMaxLength(300);
        b.Property(x => x.Description).HasColumnName("description").HasMaxLength(2000);

        b.Property(x => x.Quantity).HasColumnName("quantity").HasPrecision(18, 3).IsRequired();
        b.Property(x => x.Unit).HasColumnName("unit").IsRequired().HasMaxLength(20);

        b.Property(x => x.UnitPrice).HasColumnName("unit_price").HasPrecision(18, 2).IsRequired();
        b.Property(x => x.TaxRate).HasColumnName("tax_rate").HasPrecision(5, 2).IsRequired();

        b.Property(x => x.LineSubtotal).HasColumnName("line_subtotal").HasPrecision(18, 2).IsRequired();
        b.Property(x => x.LineTax).HasColumnName("line_tax").HasPrecision(18, 2).IsRequired();
        b.Property(x => x.LineTotal).HasColumnName("line_total").HasPrecision(18, 2).IsRequired();

        b.Property(x => x.SortOrder).HasColumnName("sort_order").IsRequired();
        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasOne(x => x.Invoice)
            .WithMany(i => i.Items)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.InvoiceId);
        b.HasIndex(x => new { x.InvoiceId, x.SortOrder });

        b.HasCheckConstraint("ck_invoice_item_qty_positive", "quantity > 0");
        b.HasCheckConstraint("ck_invoice_item_sort_nonneg", "sort_order >= 0");
        b.HasCheckConstraint("ck_invoice_item_amounts_nonneg", "unit_price >= 0 AND line_subtotal >= 0 AND line_tax >= 0 AND line_total >= 0");
        b.HasCheckConstraint("ck_invoice_item_total_match", "line_total = line_subtotal + line_tax");
    }
}
