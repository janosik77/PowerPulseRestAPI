using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class MaterialImageConfiguration
    {
        public void Configure(EntityTypeBuilder<MaterialImage> b)
        {
            b.ToTable("material_images");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.MaterialId)
                .HasColumnName("material_id")
                .IsRequired();

            b.Property(x => x.Url)
                .HasColumnName("url")
                .IsRequired()
                .HasMaxLength(2048);

            b.Property(x => x.IsPrimary)
                .HasColumnName("is_primary")
                .IsRequired();

            b.Property(x => x.AltText)
                .HasColumnName("alt_text")
                .HasMaxLength(300);

            b.Property(x => x.SortOrder)
                .HasColumnName("sort_order")
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.HasOne(x => x.Material)
                .WithMany(m => m.Images)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => x.MaterialId);
            b.HasIndex(x => new { x.MaterialId, x.SortOrder });

            b.HasCheckConstraint("ck_material_image_sort_nonneg", "sort_order >= 0");

            // Opcjonalnie: jedno primary zdjęcie na materiał (partial unique index – zależne od DB)
            // b.HasIndex(x => x.MaterialId).IsUnique().HasFilter("is_primary = true");
        }
    }
}
