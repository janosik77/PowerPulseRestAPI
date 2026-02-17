using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class ProjectAttachmentConfiguration
    {
        public void Configure(EntityTypeBuilder<ProjectAttachment> b)
        {
            b.ToTable("project_attachments");

            b.HasKey(x => x.Id);

            b.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(2048);

            b.Property(x => x.Caption)
                .HasMaxLength(500);

            b.Property(x => x.CreatedAt)
                .IsRequired();

            // Enum jako int jest OK (default EF). Jeśli wolisz string, trzeba HasConversion<string>().
            b.Property(x => x.AttachmentType)
                .IsRequired();

            // Indeksy typowe dla frontu (lista załączników projektu)
            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => new { x.ProjectId, x.CreatedAt });
            b.HasIndex(x => x.WorkSessionId);
            b.HasIndex(x => x.CreatedByUserId);

            // FK: ProjectAttachment -> Project
            b.HasOne(x => x.Project)
                .WithMany(p => p.Attachments) 
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // FK: ProjectAttachment -> WorkSession (opcjonalne)
            b.HasOne(x => x.WorkSession)
                .WithMany()
                .HasForeignKey(x => x.WorkSessionId)
                .OnDelete(DeleteBehavior.SetNull);

            // FK: ProjectAttachment -> User (CreatedBy)
            b.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
