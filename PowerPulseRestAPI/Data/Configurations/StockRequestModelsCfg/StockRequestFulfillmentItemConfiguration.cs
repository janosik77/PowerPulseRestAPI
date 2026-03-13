using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

public class StockRequestFulfillmentItemConfiguration : IEntityTypeConfiguration<StockRequestFulfillmentItem>
{
    public void Configure(EntityTypeBuilder<StockRequestFulfillmentItem> b)
    {
        b.ToTable("stock_request_fulfillment_items");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.FulfillmentId).HasColumnName("fulfillment_id").IsRequired();

        b.Property(x => x.ItemType)
            .HasColumnName("item_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.MaterialId).HasColumnName("material_id");
        b.Property(x => x.Quantity).HasColumnName("quantity").HasPrecision(18, 3);
        b.Property(x => x.Unit).HasColumnName("unit").HasMaxLength(20);

        b.Property(x => x.FromStorageLocationId).HasColumnName("from_storage_location_id");
        b.Property(x => x.ToolAssetId).HasColumnName("tool_asset_id");

        b.Property(x => x.Note).HasColumnName("note").HasMaxLength(1000);
        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasOne(x => x.Fulfillment)
            .WithMany(f => f.Items)
            .HasForeignKey(x => x.FulfillmentId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Material)
            .WithMany(m => m.StockRequestFulfillmentItems)
            .HasForeignKey(x => x.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.FromStorageLocation)
            .WithMany(sl => sl.StockRequestFulfillmentItemsFromHere)
            .HasForeignKey(x => x.FromStorageLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ToolAsset)
            .WithMany(a => a.StockRequestFulfillmentItems)
            .HasForeignKey(x => x.ToolAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.FulfillmentId);
        b.HasIndex(x => x.ItemType);
        b.HasIndex(x => x.MaterialId);
        b.HasIndex(x => x.FromStorageLocationId);
        b.HasIndex(x => x.ToolAssetId);

        b.HasCheckConstraint(
            "ck_stock_fulfillment_item_type_fields",
            "(" +
            " (item_type = 'MATERIAL' AND material_id IS NOT NULL AND quantity IS NOT NULL AND quantity > 0 AND unit IS NOT NULL AND unit <> '' AND from_storage_location_id IS NOT NULL AND tool_asset_id IS NULL)" +
            " OR " +
            " (item_type = 'TOOL' AND tool_asset_id IS NOT NULL AND material_id IS NULL AND quantity IS NULL AND unit IS NULL)" +
            ")"
        );
    }
}
