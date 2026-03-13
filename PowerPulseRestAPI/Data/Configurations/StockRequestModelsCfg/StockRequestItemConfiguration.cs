using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

public class StockRequestItemConfiguration : IEntityTypeConfiguration<StockRequestItem>
{
    public void Configure(EntityTypeBuilder<StockRequestItem> b)
    {
        b.ToTable("stock_request_items");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .HasColumnName("id");

        b.Property(x => x.StockRequestId)
            .HasColumnName("stock_request_id")
            .IsRequired();

        b.Property(x => x.ItemType)
            .HasColumnName("item_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.MaterialId)
            .HasColumnName("material_id");

        b.Property(x => x.RequestedQuantity)
            .HasColumnName("requested_quantity")
            .HasPrecision(18, 3);

        b.Property(x => x.FulfilledQuantity)
            .HasColumnName("fulfilled_quantity")
            .HasPrecision(18, 3)
            .HasDefaultValue(0m);

        b.Property(x => x.ReceivedQuantity)
            .HasColumnName("received_quantity")
            .HasPrecision(18, 3)
            .HasDefaultValue(0m);

        b.Property(x => x.Unit)
            .HasColumnName("unit")
            .HasMaxLength(20);

        b.Property(x => x.ToolId)
            .HasColumnName("tool_id");

        b.Property(x => x.RequestedCount)
            .HasColumnName("requested_count");

        b.Property(x => x.FulfilledCount)
            .HasColumnName("fulfilled_count")
            .HasDefaultValue(0);

        b.Property(x => x.ReceivedCount)
            .HasColumnName("received_count")
            .HasDefaultValue(0);

        b.Property(x => x.Note)
            .HasColumnName("note")
            .HasMaxLength(1000);

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.HasOne(x => x.StockRequest)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.StockRequestId)
                .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Material)
            .WithMany()
            .HasForeignKey(x => x.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Tool)
            .WithMany()
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.StockRequestId);
        b.HasIndex(x => x.ItemType);
        b.HasIndex(x => x.MaterialId);
        b.HasIndex(x => x.ToolId);
        b.HasIndex(x => new { x.StockRequestId, x.ItemType });

        b.HasCheckConstraint(
            "ck_stock_request_item_type_fields",
            "(" +
            " (item_type = 'MATERIAL' " +
            "   AND material_id IS NOT NULL " +
            "   AND tool_id IS NULL " +
            "   AND requested_quantity IS NOT NULL AND requested_quantity > 0 " +
            "   AND unit IS NOT NULL AND unit <> '' " +
            "   AND requested_count IS NULL " +
            " )" +
            " OR " +
            " (item_type = 'TOOL' " +
            "   AND tool_id IS NOT NULL " +
            "   AND material_id IS NULL " +
            "   AND requested_count IS NOT NULL AND requested_count > 0 " +
            "   AND requested_quantity IS NULL " +
            "   AND unit IS NULL " +
            " )" +
            ")"
        );

        b.HasCheckConstraint(
            "ck_stock_request_item_material_progress",
            "(" +
            " item_type <> 'MATERIAL' OR (" +
            "   (fulfilled_quantity IS NULL OR fulfilled_quantity >= 0) AND " +
            "   (received_quantity IS NULL OR received_quantity >= 0) AND " +
            "   (fulfilled_quantity IS NULL OR requested_quantity IS NULL OR fulfilled_quantity <= requested_quantity) AND " +
            "   (received_quantity IS NULL OR fulfilled_quantity IS NULL OR received_quantity <= fulfilled_quantity)" +
            " )" +
            ")"
        );

        b.HasCheckConstraint(
            "ck_stock_request_item_tool_progress",
            "(" +
            " item_type <> 'TOOL' OR (" +
            "   (fulfilled_count IS NULL OR fulfilled_count >= 0) AND " +
            "   (received_count IS NULL OR received_count >= 0) AND " +
            "   (fulfilled_count IS NULL OR requested_count IS NULL OR fulfilled_count <= requested_count) AND " +
            "   (received_count IS NULL OR fulfilled_count IS NULL OR received_count <= fulfilled_count)" +
            " )" +
            ")"
        );
    }
}
