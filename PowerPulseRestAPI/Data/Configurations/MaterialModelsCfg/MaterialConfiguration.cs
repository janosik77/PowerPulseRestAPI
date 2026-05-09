using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.MaterialsModels;

namespace PowerPulseRestAPI.Data.Configurations.MaterialModelsCfg
{
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> b)
        {
            b.ToTable("materials");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(300);

            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(4000);

            b.Property(x => x.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            b.Property(x => x.Manufacturer)
                .HasColumnName("manufacturer")
                .HasMaxLength(200);

            b.Property(x => x.Barcode)
                .HasColumnName("barcode")
                .HasMaxLength(100);

            b.Property(x => x.DefaultUnit)
                .HasColumnName("default_unit")
                .IsRequired()
                .HasMaxLength(20);

            b.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            b.Property(x => x.Url)
                .HasColumnName("url")
                .IsRequired()
                .HasMaxLength(2048);

            b.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();

            b.Property(x => x.Currency)
                .HasColumnName("currency")
                .HasMaxLength(10)
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired()
                .HasDefaultValue(false);

            b.HasOne(x => x.Category)
                .WithMany(x => x.Materials)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(x => x.Movements)
                .WithOne(x => x.Material)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);


            b.HasIndex(x => x.CategoryId);
            b.HasIndex(x => x.Name);
            b.HasIndex(x => x.IsActive);

            b.HasIndex(x => x.Barcode)
                .IsUnique();

            b.HasCheckConstraint("ck_material_name_not_empty", "name <> ''");
            b.HasCheckConstraint("ck_material_default_unit_not_empty", "default_unit <> ''");
            b.HasCheckConstraint("ck_material_url_not_empty", "url <> ''");
            b.HasCheckConstraint("ck_material_currency_not_empty", "currency <> ''");
            b.HasCheckConstraint("ck_material_price_nonneg", "price >= 0");
        }
    }
}