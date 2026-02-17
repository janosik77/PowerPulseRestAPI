using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ToolsModels;

public class ToolImageConfiguration : IEntityTypeConfiguration<ToolImage>
{
    public void Configure(EntityTypeBuilder<ToolImage> b)
    {
        b.ToTable("tool_images");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ToolId)
            .HasColumnName("tool_id")
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

        b.HasOne(x => x.Tool)
            .WithMany(t => t.Images)
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.ToolId);
        b.HasIndex(x => new { x.ToolId, x.SortOrder });

        b.HasCheckConstraint("ck_tool_image_sort_nonneg", "sort_order >= 0");

        // opcjonalnie: jedno primary na narzędzie (partial unique index zależny od DB)
        // b.HasIndex(x => x.ToolId).IsUnique().HasFilter("is_primary = true");
    }
}
