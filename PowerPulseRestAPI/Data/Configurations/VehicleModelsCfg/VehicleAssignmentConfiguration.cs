using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.VehicleModels;

public class VehicleAssignmentConfiguration : IEntityTypeConfiguration<VehicleAssignment>
{
    public void Configure(EntityTypeBuilder<VehicleAssignment> b)
    {
        b.ToTable("vehicle_assignments");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.VehicleId).HasColumnName("vehicle_id").IsRequired();
        b.Property(x => x.EmployeeId).HasColumnName("employee_id").IsRequired();

        b.Property(x => x.AssignedAt).HasColumnName("assigned_at").IsRequired();
        b.Property(x => x.ReturnedAt).HasColumnName("returned_at");

        b.Property(x => x.Note)
            .HasColumnName("note")
            .HasMaxLength(2000);

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasOne(x => x.Vehicle)
            .WithMany(v => v.Assignments)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Employee)
            .WithMany(e => e.VehicleAssignments)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.VehicleId)
            .IsUnique()
            .HasFilter("[returned_at] IS NULL");
        b.HasIndex(x => x.EmployeeId)
            .IsUnique()
            .HasFilter("[returned_at] IS NULL");
        b.HasIndex(x => x.EmployeeId);
        b.HasIndex(x => new { x.VehicleId, x.AssignedAt });

        b.HasCheckConstraint(
            "ck_vehicle_assignment_return_after_assign",
            "returned_at IS NULL OR returned_at >= assigned_at"
        );

        // Opcjonalnie: jedno aktywne przypisanie na auto (partial unique index zależny od DB)
        // b.HasIndex(x => x.VehicleId).IsUnique().HasFilter("returned_at IS NULL");
    }
}
