using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class MaterialProjectBalanceConfiguration
    {
        public void Configure(EntityTypeBuilder<MaterialProjectBalance> b)
        {
            b.ToTable("material_project_balance");

            b.HasKey(x => new { x.MaterialId, x.ProjectId });

            b.Property(x => x.MaterialId).HasColumnName("material_id").IsRequired();
            b.Property(x => x.ProjectId).HasColumnName("project_id").IsRequired();

            b.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .HasPrecision(18, 3)
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.HasOne(x => x.Material)
                .WithMany(m => m.ProjectBalances)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Project)
                .WithMany(p => p.MaterialBalances)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.ProjectId);
            b.HasCheckConstraint("ck_material_project_balance_qty_nonneg", "quantity >= 0");
        }
    }
}
