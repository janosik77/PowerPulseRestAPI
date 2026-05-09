using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations.ProjectModelsCfg
{
    public class ProjectAttachmentConfiguration : IEntityTypeConfiguration<ProjectAttachment>
    {
        public void Configure(EntityTypeBuilder<ProjectAttachment> b)
        {
            b.ToTable("project_attachments");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            b.Property(x => x.CreatedByEmployeeId)
                .HasColumnName("created_by_employee_id")
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

            b.Property(x => x.AttachmentType)
                .HasColumnName("attachment_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => new { x.ProjectId, x.CreatedAt });
            b.HasIndex(x => x.CreatedByEmployeeId);

            b.HasOne(x => x.Project)
                .WithMany(p => p.Attachments)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.CreatedByEmployee)
                .WithMany()
                .HasForeignKey(x => x.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasCheckConstraint("ck_project_attachment_url_not_empty", "url <> ''");
        }
    }
}