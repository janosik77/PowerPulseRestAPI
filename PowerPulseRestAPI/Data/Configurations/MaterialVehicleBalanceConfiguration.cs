using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class MaterialVehicleBalanceConfiguration
    {
        public void Configure(EntityTypeBuilder<MaterialVehicleBalance> b)
        {
            b.ToTable("material_vehicle_balance");

            b.HasKey(x => new { x.MaterialId, x.VehicleId });

            b.Property(x => x.MaterialId).HasColumnName("material_id").IsRequired();
            b.Property(x => x.VehicleId).HasColumnName("vehicle_id").IsRequired();

            b.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .HasPrecision(18, 3)
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.HasOne(x => x.Material)
                .WithMany(m => m.VehicleBalances)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Vehicle)
                .WithMany(v => v.MaterialBalances)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);


            b.HasIndex(x => x.VehicleId);
            b.HasCheckConstraint("ck_material_vehicle_balance_qty_nonneg", "quantity >= 0");
        }
    }
}
