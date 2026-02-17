using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

public class StockRequestReservationConfiguration : IEntityTypeConfiguration<StockRequestReservation>
{
    public void Configure(EntityTypeBuilder<StockRequestReservation> b)
    {
        b.ToTable("stock_request_reservations");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.StockRequestItemId).HasColumnName("stock_request_item_id").IsRequired();

        b.Property(x => x.ItemType)
            .HasColumnName("item_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.MaterialId).HasColumnName("material_id");
        b.Property(x => x.ReservedQuantity).HasColumnName("reserved_quantity").HasPrecision(18, 3);

        b.Property(x => x.StorageLocationId).HasColumnName("storage_location_id");
        b.Property(x => x.ToolAssetId).HasColumnName("tool_asset_id");

        b.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.ReservedAt).HasColumnName("reserved_at").IsRequired();
        b.Property(x => x.ReservedByUserId).HasColumnName("reserved_by_user_id").IsRequired();

        b.Property(x => x.ExpiresAt).HasColumnName("expires_at");

        b.Property(x => x.Note).HasColumnName("note").HasMaxLength(2000);

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        b.HasOne(x => x.StockRequestItem)
            .WithMany(i => i.Reservations)
            .HasForeignKey(x => x.StockRequestItemId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Material)
            .WithMany(m => m.StockRequestReservations)
            .HasForeignKey(x => x.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.StorageLocation)
            .WithMany(sl => sl.StockRequestReservationsHere)
            .HasForeignKey(x => x.StorageLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ToolAsset)
            .WithMany(a => a.StockRequestReservations)
            .HasForeignKey(x => x.ToolAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ReservedByUser)
            .WithMany(u => u.StockRequestReservations)
            .HasForeignKey(x => x.ReservedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.StockRequestItemId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.ReservedAt);
        b.HasIndex(x => x.StorageLocationId);
        b.HasIndex(x => x.ToolAssetId);

        b.HasCheckConstraint(
            "ck_stock_reservation_expires_after_reserved",
            "expires_at IS NULL OR expires_at >= reserved_at"
        );

        // MATERIAL -> material_id + reserved_quantity; tool_asset_id NULL
        // TOOL -> tool_asset_id; material_id/reserved_quantity NULL
        b.HasCheckConstraint(
            "ck_stock_reservation_item_type_fields",
            "(" +
            " (item_type = 'MATERIAL' AND material_id IS NOT NULL AND reserved_quantity IS NOT NULL AND reserved_quantity > 0 AND tool_asset_id IS NULL)" +
            " OR " +
            " (item_type = 'TOOL' AND tool_asset_id IS NOT NULL AND material_id IS NULL AND reserved_quantity IS NULL)" +
            ")"
        );
    }
}
