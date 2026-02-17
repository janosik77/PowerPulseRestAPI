using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class ProjectNoteConfiguration
    {
        public void Configure(EntityTypeBuilder<ProjectNote> b)
        {
            b.ToTable("project_notes");

            b.HasKey(x => x.Id);

            b.Property(x => x.Content)
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .IsRequired();

            // Przydatne indeksy do widoków frontu:
            // - lista notatek projektu (sort po dacie)
            // - lista notatek dla work session
            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => new { x.ProjectId, x.CreatedAt });
            b.HasIndex(x => x.WorkSessionId);
            b.HasIndex(x => x.CreatedByUserId);

            // FK: ProjectNote -> Project
            // Notatki to "dzieci" projektu, więc Cascade jest OK.
            b.HasOne(x => x.Project)
                .WithMany(p => p.Notes) 
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // FK: ProjectNote -> WorkSession (opcjonalne)
            // Jeśli usuniesz work session, notatka powinna zostać, ale odpiąć powiązanie.
            b.HasOne(x => x.WorkSession)
                .WithMany()
                .HasForeignKey(x => x.WorkSessionId)
                .OnDelete(DeleteBehavior.SetNull);

            // FK: ProjectNote -> User (CreatedBy)
            // Nie chcesz kaskadowo usuwać notatek po usunięciu usera.
            b.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

             b.HasCheckConstraint("ck_project_notes_updated_at", "updated_at >= created_at");
        }
    }
}
