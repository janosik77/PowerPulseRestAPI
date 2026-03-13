using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.ToolsModels;

public class ToolAssignmentConfiguration : IEntityTypeConfiguration<ToolAssignment>
{
    public void Configure(EntityTypeBuilder<ToolAssignment> b)
    {
        b.ToTable("tool_assignments");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .HasColumnName("id");

        b.Property(x => x.ToolId)
            .HasColumnName("tool_id")
            .IsRequired();

        b.Property(x => x.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired();

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

        b.HasOne(x => x.Tool)
                .WithMany(t => t.Assignments)
                .HasForeignKey(x => x.ToolId)
                .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Employee)
            .WithMany(e => e.ToolAssignments)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.CreatedByUser)
            .WithMany(u => u.CreatedToolAssignments)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.ToolId);
        b.HasIndex(x => x.EmployeeId);
        b.HasIndex(x => x.AssignedAt);
        b.HasIndex(x => x.ReturnedAt);
        b.HasIndex(x => x.CreatedByUserId);
        b.HasIndex(x => new { x.EmployeeId, x.ReturnedAt });
        b.HasIndex(x => x.ToolId)
                .IsUnique()
                .HasFilter("[returned_at] IS NULL");

        // Constraint: returned_at >= assigned_at
        b.HasCheckConstraint(
            "ck_tool_assignment_return_after_assign",
            "returned_at IS NULL OR returned_at >= assigned_at"
        );
    }
}
