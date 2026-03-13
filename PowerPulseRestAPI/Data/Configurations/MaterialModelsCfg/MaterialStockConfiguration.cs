using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.MaterialsModels;

namespace PowerPulseRestAPI.Data.Configurations.MaterialModelsCfg
{
    public class MaterialStockConfiguration : IEntityTypeConfiguration<MaterialStock>
    {
        public void Configure(EntityTypeBuilder<MaterialStock> b)
        {
            b.ToTable("material_stock");

            b.HasKey(x => x.MaterialId);

            b.Property(x => x.MaterialId)
                .HasColumnName("material_id")
                .IsRequired();

            b.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .HasPrecision(18, 3)
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.Property(x => x.RowVersion)
                .HasColumnName("row_version")
                .IsRowVersion();

            b.HasOne(x => x.Material)
                .WithOne(x => x.Stock)
                .HasForeignKey<MaterialStock>(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => x.MaterialId);

            b.HasIndex(x => x.MaterialId).IsUnique();

            b.HasCheckConstraint("ck_material_stock_qty_nonneg", "quantity >= 0");
        }
    }
}
