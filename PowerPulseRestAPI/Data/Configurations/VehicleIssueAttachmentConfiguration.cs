using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.VehicleModels;

public class VehicleIssueAttachmentConfiguration : IEntityTypeConfiguration<VehicleIssueAttachment>
{
    public void Configure(EntityTypeBuilder<VehicleIssueAttachment> b)
    {
        b.ToTable("vehicle_issue_attachments");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.VehicleIssueId).HasColumnName("vehicle_issue_id").IsRequired();

        b.Property(x => x.Url)
            .HasColumnName("url")
            .IsRequired()
            .HasMaxLength(2048);

        b.Property(x => x.Caption)
            .HasColumnName("caption")
            .HasMaxLength(500);

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasOne(x => x.VehicleIssue)
            .WithMany(i => i.Attachments)
            .HasForeignKey(x => x.VehicleIssueId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.VehicleIssueId);
    }
}
