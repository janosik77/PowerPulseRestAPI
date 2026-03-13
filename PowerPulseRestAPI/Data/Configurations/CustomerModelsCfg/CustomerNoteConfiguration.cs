using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.CustomerModels;

namespace PowerPulseRestAPI.Data.Configurations.CustomerModelsCfg
{
    public class CustomerNoteConfiguration : IEntityTypeConfiguration<CustomerNote>
    {
        public void Configure(EntityTypeBuilder<CustomerNote> b)
        {
            b.ToTable("customer_notes");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();

            b.Property(x => x.NoteType)
                .HasColumnName("note_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.Content)
                .HasColumnName("content")
                .IsRequired()
                .HasMaxLength(8000);

            b.Property(x => x.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.HasOne(x => x.Customer)
                .WithMany(c => c.Notes)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.CreatedByUser)
                .WithMany(u => u.CustomerNotes)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.CustomerId);
            b.HasIndex(x => x.CreatedByUserId);
            b.HasIndex(x => new { x.CustomerId, x.CreatedAt });
            b.HasIndex(x => x.NoteType);

            b.HasCheckConstraint("ck_customer_note_content_not_empty", "content <> ''");
        }
    }
}
