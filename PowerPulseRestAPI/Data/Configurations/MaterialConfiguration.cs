using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class MaterialConfiguration
    {
        public void Configure(EntityTypeBuilder<Material> b)
        {
            b.ToTable("materials");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.Sku)
                .HasColumnName("sku")
                .HasMaxLength(80);

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

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.HasOne(x => x.Category)
                .WithMany(c => c.Materials)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indeksy / unikalności (opcjonalnie zależnie od biznesu)
            b.HasIndex(x => x.CategoryId);
            b.HasIndex(x => x.Name);

            b.HasIndex(x => x.Sku).IsUnique();      // jeśli SKU ma być unikalne (dla NULL zależy od DB)
            b.HasIndex(x => x.Barcode).IsUnique();  // jeśli barcode ma być unikalny

            // Constrainty
            b.HasCheckConstraint("ck_material_default_unit_not_empty", "default_unit <> ''");
        }
    }
}
