using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

public class StockRequestHistoryConfiguration : IEntityTypeConfiguration<StockRequestHistory>
{
    public void Configure(EntityTypeBuilder<StockRequestHistory> b)
    {
        b.ToTable("stock_request_history");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.StockRequestId).HasColumnName("stock_request_id").IsRequired();

        b.Property(x => x.OldStatus)
            .HasColumnName("old_status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.NewStatus)
            .HasColumnName("new_status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.Note).HasColumnName("note").HasMaxLength(2000);

        b.Property(x => x.ChangedByUserId).HasColumnName("changed_by_user_id").IsRequired();
        b.Property(x => x.ChangedAt).HasColumnName("changed_at").IsRequired();

        b.HasOne(x => x.StockRequest)
            .WithMany(sr => sr.History)
            .HasForeignKey(x => x.StockRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.ChangedByUser)
            .WithMany(u => u.StockRequestChanges)
            .HasForeignKey(x => x.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.StockRequestId);
        b.HasIndex(x => x.ChangedAt);
        b.HasIndex(x => new { x.StockRequestId, x.ChangedAt });
    }
}
