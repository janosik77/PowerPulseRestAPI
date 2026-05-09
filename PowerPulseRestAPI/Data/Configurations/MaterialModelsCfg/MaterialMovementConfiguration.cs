using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.MaterialsModels;

namespace PowerPulseRestAPI.Data.Configurations.MaterialModelsCfg
{
    public class MaterialMovementConfiguration : IEntityTypeConfiguration<MaterialMovement>
    {
        public void Configure(EntityTypeBuilder<MaterialMovement> b)
        {
            b.ToTable("material_movements");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.MaterialId)
                .HasColumnName("material_id")
                .IsRequired();

            b.Property(x => x.MovementType)
                .HasColumnName("movement_type")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            b.Property(x => x.ProjectId)
                .HasColumnName("project_id");

            b.Property(x => x.OperationId)
                .HasColumnName("operation_id")
                .IsRequired();

            b.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .HasPrecision(18, 3)
                .IsRequired();

            b.Property(x => x.Unit)
                .HasColumnName("unit")
                .HasMaxLength(20)
                .IsRequired();

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

            b.Property(x => x.InvoiceId)
                .HasColumnName("invoice_id");

            b.Property(x => x.InvoicedAt)
                .HasColumnName("invoiced_at");

            b.HasOne(x => x.Material)
                .WithMany(x => x.Movements)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Project)
                .WithMany(x => x.MaterialMovements)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CreatedMaterialMovements)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Invoice)
                .WithMany(i => i.MaterialMovements)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.SetNull);

            b.HasIndex(x => x.MaterialId);
            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => x.OperationId);
            b.HasIndex(x => x.OccurredAt);
            b.HasIndex(x => x.CreatedAt);
            b.HasIndex(x => x.MovementType);
            b.HasIndex(x => new { x.MaterialId, x.ProjectId });
            b.HasIndex(x => new { x.OperationId, x.MaterialId });
            b.HasIndex(x => x.InvoiceId);
            b.HasIndex(x => x.InvoicedAt);

            b.HasCheckConstraint("ck_material_movement_qty_positive", "quantity > 0");
            b.HasCheckConstraint("ck_material_movement_unit_not_empty", "unit <> ''");
            b.HasCheckConstraint("ck_material_movement_invoiced_at_requires_invoice",
                "invoice_id IS NOT NULL OR invoiced_at IS NULL");

        }
    }
}