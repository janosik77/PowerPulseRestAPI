using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

public class StockRequestItemConfiguration : IEntityTypeConfiguration<StockRequestItem>
{
    public void Configure(EntityTypeBuilder<StockRequestItem> b)
    {
        b.ToTable("stock_request_items");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.StockRequestId).HasColumnName("stock_request_id").IsRequired();

        b.Property(x => x.ItemType)
            .HasColumnName("item_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.MaterialId).HasColumnName("material_id");
        b.Property(x => x.RequestedQuantity).HasColumnName("requested_quantity").HasPrecision(18, 3);
        b.Property(x => x.Unit).HasColumnName("unit").HasMaxLength(20);

        b.Property(x => x.ToolId).HasColumnName("tool_id");
        b.Property(x => x.RequestedCount).HasColumnName("requested_count");

        b.Property(x => x.Note).HasColumnName("note").HasMaxLength(1000);

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasOne(x => x.StockRequest)
            .WithMany(sr => sr.Items)
            .HasForeignKey(x => x.StockRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Material)
            .WithMany(m => m.StockRequestItems)
            .HasForeignKey(x => x.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Tool)
            .WithMany(t => t.StockRequestItems)
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.StockRequestId);
        b.HasIndex(x => x.ItemType);
        b.HasIndex(x => x.MaterialId);
        b.HasIndex(x => x.ToolId);

        // MATERIAL -> material_id + requested_quantity + unit; tool_id NULL; requested_count NULL
        // TOOL -> tool_id + requested_count; material_id NULL; requested_quantity/unit NULL
        b.HasCheckConstraint(
            "ck_stock_request_item_type_fields",
            "(" +
            " (item_type = 'MATERIAL' AND material_id IS NOT NULL AND tool_id IS NULL AND requested_quantity IS NOT NULL AND requested_quantity > 0 AND unit IS NOT NULL AND unit <> '' AND requested_count IS NULL)" +
            " OR " +
            " (item_type = 'TOOL' AND tool_id IS NOT NULL AND material_id IS NULL AND requested_count IS NOT NULL AND requested_count > 0 AND requested_quantity IS NULL AND unit IS NULL)" +
            ")"
        );
    }
}
