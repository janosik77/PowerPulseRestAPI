using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class MaterialMovementConfiguration
    {
        public void Configure(EntityTypeBuilder<MaterialMovement> b)
        {
            b.ToTable("material_movements");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.MaterialId)
                .HasColumnName("material_id")
                .IsRequired();

            b.Property(x => x.TransactionType)
                .HasColumnName("transaction_type")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            b.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .HasPrecision(18, 3)
                .IsRequired();

            b.Property(x => x.Unit)
                .HasColumnName("unit")
                .IsRequired()
                .HasMaxLength(20);

            b.Property(x => x.FromStorageLocationId).HasColumnName("from_storage_location_id");
            b.Property(x => x.ToStorageLocationId).HasColumnName("to_storage_location_id");
            b.Property(x => x.ToProjectId).HasColumnName("to_project_id");
            b.Property(x => x.ToVehicleId).HasColumnName("to_vehicle_id");
            b.Property(x => x.ToEmployeeId).HasColumnName("to_employee_id");

            b.Property(x => x.Note)
                .HasColumnName("note")
                .HasMaxLength(2000);

            b.Property(x => x.OccurredAt)
                .HasColumnName("occurred_at")
                .IsRequired();

            b.Property(x => x.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            // Relacje
            b.HasOne(x => x.Material)
                .WithMany(m => m.Movements)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.FromStorageLocation)
                .WithMany(sl => sl.MovementsFrom)
                .HasForeignKey(x => x.FromStorageLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.ToStorageLocation)
                .WithMany(sl => sl.MovementsTo)
                .HasForeignKey(x => x.ToStorageLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.ToProject)
                .WithMany(p => p.MaterialMovements)
                .HasForeignKey(x => x.ToProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.ToVehicle)
                .WithMany(v => v.MaterialMovements)
                .HasForeignKey(x => x.ToVehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.ToEmployee)
                .WithMany(e => e.MaterialMovements)
                .HasForeignKey(x => x.ToEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.CreatedByUser)
                .WithMany(u => u.CreatedMaterialMovements)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            // Indeksy (historia ruchów)
            b.HasIndex(x => x.MaterialId);
            b.HasIndex(x => x.OccurredAt);
            b.HasIndex(x => x.CreatedByUserId);
            b.HasIndex(x => new { x.MaterialId, x.OccurredAt });

            b.HasIndex(x => x.FromStorageLocationId);
            b.HasIndex(x => x.ToStorageLocationId);
            b.HasIndex(x => x.ToProjectId);
            b.HasIndex(x => x.ToVehicleId);
            b.HasIndex(x => x.ToEmployeeId);

            // Constrainty
            b.HasCheckConstraint("ck_material_movement_qty_positive", "quantity > 0");
            b.HasCheckConstraint("ck_material_movement_unit_not_empty", "unit <> ''");

            // Dokładnie jedno pole docelowe (to_*) może być ustawione
            b.HasCheckConstraint(
                "ck_material_movement_single_target",
                "(" +
                " (CASE WHEN to_storage_location_id IS NULL THEN 0 ELSE 1 END) + " +
                " (CASE WHEN to_project_id IS NULL THEN 0 ELSE 1 END) + " +
                " (CASE WHEN to_vehicle_id IS NULL THEN 0 ELSE 1 END) + " +
                " (CASE WHEN to_employee_id IS NULL THEN 0 ELSE 1 END) " +
                ") = 1"
            );

            // (opcjonalnie) jeśli chcesz wymusić, że TRANSFER ma from_storage_location_id itd.
            // tego nie da się łatwo uogólnić bez zależności od TransactionType,
            // ale można dodać per-typ osobne CHECKi jeśli chcesz.
        }
    }
}
