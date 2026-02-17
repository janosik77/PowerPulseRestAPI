using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ToolsModels;

public class ToolIssueAttachmentConfiguration : IEntityTypeConfiguration<ToolIssueAttachment>
{
    public void Configure(EntityTypeBuilder<ToolIssueAttachment> b)
    {
        b.ToTable("tool_issue_attachments");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ToolIssueId)
            .HasColumnName("tool_issue_id")
            .IsRequired();

        b.Property(x => x.Url)
            .HasColumnName("url")
            .IsRequired()
            .HasMaxLength(2048);

        b.Property(x => x.Caption)
            .HasColumnName("caption")
            .HasMaxLength(500);

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.HasOne(x => x.ToolIssue)
            .WithMany(i => i.Attachments)
            .HasForeignKey(x => x.ToolIssueId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.ToolIssueId);
    }
}
