using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations.ProjectModelsCfg
{
    public class ProjectNoteConfiguration : IEntityTypeConfiguration<ProjectNote>
    {
        public void Configure(EntityTypeBuilder<ProjectNote> b)
        {
            b.ToTable("project_notes");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            b.Property(x => x.WorkSessionId)
                .HasColumnName("work_session_id");

            b.Property(x => x.Content)
                .HasColumnName("content")
                .IsRequired()
                .HasMaxLength(4000);

            b.Property(x => x.NoteType)
                .HasColumnName("note_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => new { x.ProjectId, x.CreatedAt });
            b.HasIndex(x => x.WorkSessionId);
            b.HasIndex(x => x.CreatedByUserId);

            b.HasOne(x => x.Project)
                .WithMany(p => p.Notes)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.WorkSession)
                .WithMany()
                .HasForeignKey(x => x.WorkSessionId)
                .OnDelete(DeleteBehavior.SetNull);

            b.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasCheckConstraint("ck_project_notes_updated_at", "updated_at >= created_at");
            b.HasCheckConstraint("ck_project_note_content_not_empty", "content <> ''");
        }
    }
}