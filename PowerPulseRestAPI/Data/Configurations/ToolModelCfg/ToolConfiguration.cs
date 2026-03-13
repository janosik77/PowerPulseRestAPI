using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ToolsModels;

public class ToolConfiguration : IEntityTypeConfiguration<Tool>
{
    public void Configure(EntityTypeBuilder<Tool> b)
    {
        b.ToTable("tools");

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

        b.Property(x => x.Model)
            .HasColumnName("model")
            .HasMaxLength(200);

        b.Property(x => x.Barcode)
            .HasColumnName("barcode")
            .HasMaxLength(100);

        b.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        b.Property(x => x.Url)
            .HasColumnName("url")
            .IsRequired()
            .HasMaxLength(2048);

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // relacja do kategorii (dwukierunkowo)
        b.HasOne(x => x.Category)
            .WithMany(c => c.Tools)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // indeksy
        b.HasIndex(x => x.CategoryId);
        b.HasIndex(x => x.Name);
        b.HasIndex(x => x.IsActive);

        // unikalności (opcjonalnie biznesowo)
        b.HasIndex(x => x.Sku).IsUnique();
        b.HasIndex(x => x.Barcode).IsUnique();

        b.HasCheckConstraint("ck_tool_name_not_empty", "name <> ''");
        b.HasCheckConstraint("ck_tool_sku_not_empty", "sku IS NULL OR sku <> ''");
        b.HasCheckConstraint("ck_tool_barcode_not_empty", "barcode IS NULL OR barcode <> ''");
    }
}
