using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.InvoiceModels;

public class InvoiceHistoryConfiguration : IEntityTypeConfiguration<InvoiceHistory>
{
    public void Configure(EntityTypeBuilder<InvoiceHistory> b)
    {
        b.ToTable("invoice_history");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.InvoiceId).HasColumnName("invoice_id").IsRequired();

        b.Property(x => x.OldStatus)
            .HasColumnName("old_status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.NewStatus)
            .HasColumnName("new_status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.Note).HasColumnName("note").HasMaxLength(2000);

        b.Property(x => x.ChangedByUserId).HasColumnName("changed_by_user_id").IsRequired();
        b.Property(x => x.ChangedAt).HasColumnName("changed_at").IsRequired();

        b.HasOne(x => x.Invoice)
            .WithMany(i => i.History)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.ChangedByUser)
            .WithMany(u => u.InvoiceChanges)
            .HasForeignKey(x => x.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.InvoiceId);
        b.HasIndex(x => x.ChangedAt);
        b.HasIndex(x => x.ChangedByUserId);
        b.HasIndex(x => new { x.InvoiceId, x.ChangedAt });
    }
}
