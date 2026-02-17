using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class MaterialCategoryConfiguration
    {
        public void Configure(EntityTypeBuilder<MaterialCategory> b)
        {
            b.ToTable("material_categories");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);

            b.Property(x => x.ParentId)
                .HasColumnName("parent_id");

            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(2000);

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            // self-reference: Parent 1 -> N Children
            b.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.Name).IsUnique();
            b.HasIndex(x => x.ParentId);

            // Opcjonalnie: nie pozwól, by kategoria wskazywała na samą siebie
            b.HasCheckConstraint("ck_material_category_not_self_parent", "parent_id IS NULL OR parent_id <> id");
        }
    }
}
