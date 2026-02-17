using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.VehicleModels;

public class VehicleIssueConfiguration : IEntityTypeConfiguration<VehicleIssue>
{
    public void Configure(EntityTypeBuilder<VehicleIssue> b)
    {
        b.ToTable("vehicle_issues");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.VehicleId).HasColumnName("vehicle_id").IsRequired();
        b.Property(x => x.ReportedByUserId).HasColumnName("reported_by_user_id").IsRequired();

        b.Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(4000);

        b.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        b.HasOne(x => x.Vehicle)
            .WithMany(v => v.Issues)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ReportedByUser)
            .WithMany(u => u.VehicleIssuesReported)
            .HasForeignKey(x => x.ReportedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.VehicleId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => new { x.VehicleId, x.Status });
    }
}
