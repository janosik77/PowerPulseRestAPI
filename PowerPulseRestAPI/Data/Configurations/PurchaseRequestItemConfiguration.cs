using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.PurchaseReqModels;

public class PurchaseRequestItemConfiguration : IEntityTypeConfiguration<PurchaseRequestItem>
{
    public void Configure(EntityTypeBuilder<PurchaseRequestItem> b)
    {
        b.ToTable("purchase_request_items");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.PurchaseRequestId).HasColumnName("purchase_request_id").IsRequired();

        b.Property(x => x.ItemType)
            .HasColumnName("item_type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.MaterialId).HasColumnName("material_id");
        b.Property(x => x.ToolId).HasColumnName("tool_id");

        b.Property(x => x.ItemName)
            .HasColumnName("item_name")
            .HasMaxLength(300);

        b.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .HasPrecision(18, 3);

        b.Property(x => x.Unit)
            .HasColumnName("unit")
            .HasMaxLength(20);

        b.Property(x => x.Note)
            .HasColumnName("note")
            .HasMaxLength(1000);

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        // Relacje (dwukierunkowe)
        b.HasOne(x => x.PurchaseRequest)
            .WithMany(pr => pr.Items)
            .HasForeignKey(x => x.PurchaseRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Material)
            .WithMany(m => m.PurchaseRequestItems)
            .HasForeignKey(x => x.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Tool)
            .WithMany(t => t.PurchaseRequestItems)
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indeksy
        b.HasIndex(x => x.PurchaseRequestId);
        b.HasIndex(x => x.ItemType);
        b.HasIndex(x => x.MaterialId);
        b.HasIndex(x => x.ToolId);

        // Constraint: logika pól zależnie od ItemType
        // MATERIAL -> material_id + quantity + unit, tool_id NULL
        // TOOL     -> tool_id, a quantity/unit/material_id NULL (ItemName opcjonalne)
        b.HasCheckConstraint(
            "ck_purchase_request_item_type_fields",
            "(" +
            " (item_type = 'MATERIAL' AND material_id IS NOT NULL AND tool_id IS NULL AND quantity IS NOT NULL AND quantity > 0 AND unit IS NOT NULL AND unit <> '')" +
            " OR " +
            " (item_type = 'TOOL' AND tool_id IS NOT NULL AND material_id IS NULL AND quantity IS NULL AND unit IS NULL)" +
            ")"
        );
    }
}
