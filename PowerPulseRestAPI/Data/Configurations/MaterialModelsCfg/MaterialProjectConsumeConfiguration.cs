using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.MaterialsModels;

namespace PowerPulseRestAPI.Data.Configurations.MaterialModelsCfg
{
    public class MaterialProjectConsumeConfiguration : IEntityTypeConfiguration<MaterialProjectConsume>
    {
        public void Configure(EntityTypeBuilder<MaterialProjectConsume> b)
        {
            b.ToTable("material_project_consumes");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            b.Property(x => x.MaterialId)
                .HasColumnName("material_id")
                .IsRequired();

            b.Property(x => x.PreviousQuantity)
                .HasColumnName("previous_quantity")
                .HasPrecision(18, 3)
                .IsRequired();

            b.Property(x => x.ActualQuantity)
                .HasColumnName("actual_quantity")
                .HasPrecision(18, 3)
                .IsRequired();

            b.Property(x => x.ConsumedQuantity)
                .HasColumnName("consumed_quantity")
                .HasPrecision(18, 3)
                .IsRequired();

            b.Property(x => x.Unit)
                .HasColumnName("unit")
                .HasMaxLength(20)
                .IsRequired();

            b.Property(x => x.InvoiceId)
                .HasColumnName("invoice_id");

            b.Property(x => x.InvoicedAt)
                .HasColumnName("invoiced_at");

            b.Property(x => x.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired();

            b.Property(x => x.InventoryBatchId)
                .HasColumnName("inventory_batch_id");

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.HasOne(x => x.Project)
                .WithMany(x => x.MaterialProjectConsumes)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Material)
                .WithMany(x => x.ProjectConsumes)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Invoice)
                .WithMany(x => x.MaterialProjectConsumes)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.SetNull);

            b.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CreatedMaterialProjectConsumes)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => x.MaterialId);
            b.HasIndex(x => x.InvoiceId);
            b.HasIndex(x => x.InventoryBatchId);
            b.HasIndex(x => x.CreatedByUserId);
            b.HasIndex(x => x.CreatedAt);
            b.HasIndex(x => new { x.ProjectId, x.MaterialId, x.CreatedAt });

            b.HasCheckConstraint("ck_material_project_consume_prev_qty_nonneg", "previous_quantity >= 0");
            b.HasCheckConstraint("ck_material_project_consume_actual_qty_nonneg", "actual_quantity >= 0");
            b.HasCheckConstraint("ck_material_project_consume_consumed_qty_nonneg", "consumed_quantity >= 0");
            b.HasCheckConstraint("ck_material_project_consume_unit_not_empty", "unit <> ''");
            b.HasCheckConstraint(
                "ck_material_project_consume_qty_formula",
                "consumed_quantity = previous_quantity - actual_quantity"
            );
        }
    }
}