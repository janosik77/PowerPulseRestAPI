using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ToolsModels;

public class ToolCategoryConfiguration : IEntityTypeConfiguration<ToolCategory>
{
    public void Configure(EntityTypeBuilder<ToolCategory> b)
    {
        b.ToTable("tool_categories");

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

        b.HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.Name).IsUnique();
        b.HasIndex(x => x.ParentId);

        b.HasCheckConstraint("ck_tool_category_not_self_parent", "parent_id IS NULL OR parent_id <> id");
    }
}
