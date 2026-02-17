using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ToolsModels;

public class ToolAssetStockConfiguration : IEntityTypeConfiguration<ToolAssetStock>
{
    public void Configure(EntityTypeBuilder<ToolAssetStock> b)
    {
        b.ToTable("tool_asset_stock");

        // PK = FK (1:1)
        b.HasKey(x => x.ToolAssetId);

        b.Property(x => x.ToolAssetId)
            .HasColumnName("tool_asset_id")
            .IsRequired();

        b.Property(x => x.StorageLocationId)
            .HasColumnName("storage_location_id");

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        b.HasOne(x => x.ToolAsset)
            .WithOne(a => a.Stock)
            .HasForeignKey<ToolAssetStock>(x => x.ToolAssetId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.StorageLocation)
            .WithMany(sl => sl.ToolAssetStocks)
            .HasForeignKey(x => x.StorageLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.StorageLocationId);
    }
}
