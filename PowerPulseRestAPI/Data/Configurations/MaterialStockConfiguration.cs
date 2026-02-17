using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class MaterialStockConfiguration
    {
        public void Configure(EntityTypeBuilder<MaterialStock> b)
        {
            b.ToTable("material_stock");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.MaterialId)
                .HasColumnName("material_id")
                .IsRequired();

            b.Property(x => x.StorageLocationId)
                .HasColumnName("storage_location_id")
                .IsRequired();

            b.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .HasPrecision(18, 3)
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.HasOne(x => x.Material)
                .WithMany(m => m.Stocks)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.StorageLocation)
                .WithMany(sl => sl.MaterialStocks)
                .HasForeignKey(x => x.StorageLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unikalny stan dla (material, location)
            b.HasIndex(x => new { x.MaterialId, x.StorageLocationId }).IsUnique();

            b.HasCheckConstraint("ck_material_stock_qty_nonneg", "quantity >= 0");
        }
    }
}
