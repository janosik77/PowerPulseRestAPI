using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class WorkSessionEventConfiguration
    {
        public void Configure(EntityTypeBuilder<WorkSessionEvent> b)
        {
            b.ToTable("work_session_events");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.WorkSessionId)
                .HasColumnName("work_session_id")
                .IsRequired();

            b.Property(x => x.EventType)
                .HasColumnName("event_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.EventAt)
                .HasColumnName("event_at")
                .IsRequired();

            b.Property(x => x.Note)
                .HasColumnName("note")
                .HasMaxLength(2000);

            b.Property(x => x.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            // RELACJE
            b.HasOne(x => x.WorkSession)
                .WithMany(ws => ws.Events) 
                .HasForeignKey(x => x.WorkSessionId)
                .OnDelete(DeleteBehavior.Cascade); // event bez sesji nie ma sensu

            b.HasOne(x => x.CreatedByUser)
                .WithMany(u => u.WorkSessionEvents) 
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // INDEKSY
            b.HasIndex(x => x.WorkSessionId);
            b.HasIndex(x => new { x.WorkSessionId, x.EventAt });
            b.HasIndex(x => x.CreatedByUserId);

            b.HasCheckConstraint(
                    "ck_work_session_event_not_future",
                    "event_at <= NOW()"
);
        }
    }
}
