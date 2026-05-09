using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.StockModels;

public class LowStockNoteConfiguration : IEntityTypeConfiguration<LowStockNote>
{
    public void Configure(EntityTypeBuilder<LowStockNote> b)
    {
        b.ToTable("low_stock_notes");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .HasColumnName("id");

        b.Property(x => x.Priority)
            .HasColumnName("priority")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.CreatedByEmployeeId)
            .HasColumnName("created_by_employee_id")
            .IsRequired();

        b.Property(x => x.Note)
            .HasColumnName("note")
            .HasMaxLength(2000)
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        b.HasOne(x => x.CreatedByEmployee)
            .WithMany(e => e.LowStockNotes)
            .HasForeignKey(x => x.CreatedByEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.Priority);
        b.HasIndex(x => x.CreatedByEmployeeId);
        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => new { x.CreatedByEmployeeId, x.CreatedAt });

        b.HasCheckConstraint(
            "ck_low_stock_note_updated_at",
            "updated_at >= created_at"
        );
    }
}
