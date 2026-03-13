using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.VehicleModels;

public class ServiceOrderHistoryConfiguration : IEntityTypeConfiguration<ServiceOrderHistory>
{
    public void Configure(EntityTypeBuilder<ServiceOrderHistory> b)
    {
        b.ToTable("service_order_history");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ServiceOrderId)
            .HasColumnName("service_order_id")
            .IsRequired();

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

        b.Property(x => x.Note)
            .HasColumnName("note")
            .HasMaxLength(2000);

        b.Property(x => x.ChangedByUserId)
            .HasColumnName("changed_by_user_id")
            .IsRequired();

        b.Property(x => x.ChangedAt)
            .HasColumnName("changed_at")
            .IsRequired();

        b.HasOne(x => x.ServiceOrder)
            .WithMany(s => s.History)
            .HasForeignKey(x => x.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.ChangedByUser)
            .WithMany(u => u.ServiceOrderChanges)
            .HasForeignKey(x => x.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.ServiceOrderId);
        b.HasIndex(x => x.ChangedByUserId);
        b.HasIndex(x => x.ChangedAt);
        b.HasIndex(x => new { x.ServiceOrderId, x.ChangedAt });
    }
}