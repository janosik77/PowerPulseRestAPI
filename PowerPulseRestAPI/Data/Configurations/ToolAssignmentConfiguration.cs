using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ToolsModels;

public class ToolAssignmentConfiguration : IEntityTypeConfiguration<ToolAssignment>
{
    public void Configure(EntityTypeBuilder<ToolAssignment> b)
    {
        b.ToTable("tool_assignments");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ToolAssetId)
            .HasColumnName("tool_asset_id")
            .IsRequired();

        b.Property(x => x.ToStorageLocationId).HasColumnName("to_storage_location_id");
        b.Property(x => x.ToVehicleId).HasColumnName("to_vehicle_id");
        b.Property(x => x.ToProjectId).HasColumnName("to_project_id");
        b.Property(x => x.ToEmployeeId).HasColumnName("to_employee_id");

        b.Property(x => x.AssignedAt)
            .HasColumnName("assigned_at")
            .IsRequired();

        b.Property(x => x.ReturnedAt)
            .HasColumnName("returned_at");

        b.Property(x => x.Notes)
            .HasColumnName("notes")
            .HasMaxLength(2000);

        b.Property(x => x.CreatedByUserId)
            .HasColumnName("created_by_user_id")
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        // Relacje (dwukierunkowe)
        b.HasOne(x => x.ToolAsset)
            .WithMany(a => a.Assignments)
            .HasForeignKey(x => x.ToolAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ToStorageLocation)
            .WithMany(sl => sl.ToolAssignmentsToHere)
            .HasForeignKey(x => x.ToStorageLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ToVehicle)
            .WithMany(v => v.ToolAssignments)
            .HasForeignKey(x => x.ToVehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ToProject)
            .WithMany(p => p.ToolAssignments)
            .HasForeignKey(x => x.ToProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ToEmployee)
            .WithMany(e => e.ToolAssignments)
            .HasForeignKey(x => x.ToEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.CreatedByUser)
            .WithMany(u => u.CreatedToolAssignments)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indeksy
        b.HasIndex(x => x.ToolAssetId);
        b.HasIndex(x => x.AssignedAt);
        b.HasIndex(x => x.ReturnedAt);
        b.HasIndex(x => x.CreatedByUserId);
        b.HasIndex(x => x.ToStorageLocationId);
        b.HasIndex(x => x.ToVehicleId);
        b.HasIndex(x => x.ToProjectId);
        b.HasIndex(x => x.ToEmployeeId);

        // Constraint: returned_at >= assigned_at
        b.HasCheckConstraint(
            "ck_tool_assignment_return_after_assign",
            "returned_at IS NULL OR returned_at >= assigned_at"
        );

        // Constraint: dokładnie jedno pole docelowe ustawione
        b.HasCheckConstraint(
            "ck_tool_assignment_single_target",
            "(" +
            " (CASE WHEN to_storage_location_id IS NULL THEN 0 ELSE 1 END) + " +
            " (CASE WHEN to_vehicle_id IS NULL THEN 0 ELSE 1 END) + " +
            " (CASE WHEN to_project_id IS NULL THEN 0 ELSE 1 END) + " +
            " (CASE WHEN to_employee_id IS NULL THEN 0 ELSE 1 END) " +
            ") = 1"
        );
    }
}
