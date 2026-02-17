using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ToolsModels;

public class ToolAssetConfiguration : IEntityTypeConfiguration<ToolAsset>
{
    public void Configure(EntityTypeBuilder<ToolAsset> b)
    {
        b.ToTable("tool_assets");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ToolId)
            .HasColumnName("tool_id")
            .IsRequired();

        b.Property(x => x.SerialNumber)
            .HasColumnName("serial_number")
            .HasMaxLength(200);

        b.Property(x => x.AssetTag)
            .HasColumnName("asset_tag")
            .HasMaxLength(200);

        b.Property(x => x.Condition)
            .HasColumnName("condition")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        b.HasOne(x => x.Tool)
            .WithMany(t => t.Assets)
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.ToolId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.Condition);

        // unikalność numerów/etykiet (jeśli chcesz)
        b.HasIndex(x => x.SerialNumber).IsUnique();
        b.HasIndex(x => x.AssetTag).IsUnique();
    }
}
