using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ToolsModels;

public class ToolIssueConfiguration : IEntityTypeConfiguration<ToolIssue>
{
    public void Configure(EntityTypeBuilder<ToolIssue> b)
    {
        b.ToTable("tool_issues");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ToolAssetId)
            .HasColumnName("tool_asset_id")
            .IsRequired();

        b.Property(x => x.ReportedByUserId)
            .HasColumnName("reported_by_user_id")
            .IsRequired();

        b.Property(x => x.IssueType)
            .HasColumnName("issue_type")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(4000);

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

        b.HasOne(x => x.ToolAsset)
            .WithMany(a => a.Issues)
            .HasForeignKey(x => x.ToolAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ReportedByUser)
            .WithMany(u => u.ReportedToolIssues)
            .HasForeignKey(x => x.ReportedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.ToolAssetId);
        b.HasIndex(x => x.ReportedByUserId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => new { x.ToolAssetId, x.Status });
    }
}
