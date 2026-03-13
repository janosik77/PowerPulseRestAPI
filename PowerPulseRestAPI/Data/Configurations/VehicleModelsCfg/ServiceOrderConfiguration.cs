using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.VehicleModels;

public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> b)
    {
        b.ToTable("service_orders");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.VehicleId)
            .HasColumnName("vehicle_id")
            .IsRequired();

        b.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(300);

        b.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(4000);

        b.Property(x => x.Priority)
            .HasColumnName("priority")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.RequestedByUserId)
            .HasColumnName("requested_by_user_id")
            .IsRequired();

        b.Property(x => x.RequestedAt)
            .HasColumnName("requested_at")
            .IsRequired();

        b.Property(x => x.ApprovalStatus)
            .HasColumnName("approval_status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.ApprovedByUserId)
            .HasColumnName("approved_by_user_id");

        b.Property(x => x.ApprovedAt)
            .HasColumnName("approved_at");

        b.Property(x => x.ServiceProviderName)
            .HasColumnName("service_provider_name")
            .HasMaxLength(200);

        b.Property(x => x.ServiceProviderPhone)
            .HasColumnName("service_provider_phone")
            .HasMaxLength(50);

        b.Property(x => x.AppointmentAt)
            .HasColumnName("appointment_at");

        b.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.MileageAtService)
            .HasColumnName("mileage_at_service");

        b.Property(x => x.FinalDescription)
            .HasColumnName("final_description")
            .HasMaxLength(4000);

        b.Property(x => x.TotalCost)
            .HasColumnName("total_cost")
            .HasColumnType("decimal(18,2)");

        b.Property(x => x.Currency)
            .HasColumnName("currency")
            .IsRequired()
            .HasMaxLength(3);

        b.Property(x => x.InvoiceNumber)
            .HasColumnName("invoice_number")
            .HasMaxLength(100);

        b.Property(x => x.CompletedByUserId)
            .HasColumnName("completed_by_user_id");

        b.Property(x => x.CompletedAt)
            .HasColumnName("completed_at");

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        b.HasOne(x => x.Vehicle)
            .WithMany(v => v.ServiceOrders)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.RequestedByUser)
            .WithMany(u => u.ServiceOrdersRequested)
            .HasForeignKey(x => x.RequestedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasOne(x => x.ApprovedByUser)
            .WithMany(u => u.ServiceOrdersApproved)
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasOne(x => x.CompletedByUser)
            .WithMany(u => u.ServiceOrdersCompleted)
            .HasForeignKey(x => x.CompletedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasIndex(x => x.VehicleId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.ApprovalStatus);
        b.HasIndex(x => x.RequestedAt);
        b.HasIndex(x => new { x.VehicleId, x.Status });

        b.HasCheckConstraint(
            "ck_service_order_mileage_nonneg",
            "[mileage_at_service] IS NULL OR [mileage_at_service] >= 0"
        );

        b.HasCheckConstraint(
            "ck_service_order_total_cost_nonneg",
            "[total_cost] IS NULL OR [total_cost] >= 0"
        );

        b.HasCheckConstraint(
            "ck_service_order_approved_after_requested",
            "[approved_at] IS NULL OR [approved_at] >= [requested_at]"
        );

        b.HasCheckConstraint(
            "ck_service_order_completed_after_requested",
            "[completed_at] IS NULL OR [completed_at] >= [requested_at]"
        );

        b.HasCheckConstraint(
            "ck_service_order_returned_after_sent",
            "[returned_from_service_at] IS NULL OR [sent_to_service_at] IS NULL OR [returned_from_service_at] >= [sent_to_service_at]"
        );
    }
}