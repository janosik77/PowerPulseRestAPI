using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.VehicleModels;

public class VehicleMileageRecordConfiguration : IEntityTypeConfiguration<VehicleMileageRecord>
{
    public void Configure(EntityTypeBuilder<VehicleMileageRecord> b)
    {
        b.ToTable("vehicle_mileage_records");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.VehicleId).HasColumnName("vehicle_id").IsRequired();

        b.Property(x => x.Mileage)
            .HasColumnName("mileage")
            .IsRequired();

        b.Property(x => x.RecordedAt)
            .HasColumnName("recorded_at")
            .IsRequired();

        b.Property(x => x.SourceType)
            .HasColumnName("source_type")
            .HasConversion<string>()
            .HasMaxLength(30)
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

        // Relacje (dwukierunkowe)
        b.HasOne(x => x.Vehicle)
            .WithMany(v => v.MileageRecords)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.CreatedByUser)
            .WithMany(u => u.VehicleMileageRecords)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indeksy
        b.HasIndex(x => x.VehicleId);
        b.HasIndex(x => x.RecordedAt);
        b.HasIndex(x => new { x.VehicleId, x.RecordedAt });

        // Constrainty
        b.HasCheckConstraint("ck_vehicle_mileage_record_nonneg", "mileage >= 0");
    }
}
