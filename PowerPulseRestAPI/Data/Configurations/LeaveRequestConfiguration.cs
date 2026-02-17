using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.EmployeeModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class LeaveRequestConfiguration
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> b)
        {
            b.ToTable("leave_requests");

            // PK
            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            // employee
            b.Property(x => x.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired();

            // enums
            b.Property(x => x.LeaveType)
                .HasColumnName("leave_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            // dates
            b.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("date")
                .IsRequired();

            b.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .HasColumnType("date")
                .IsRequired();

            // reason
            b.Property(x => x.Reason)
                .HasColumnName("reason")
                .HasMaxLength(1000);

            // requester
            b.Property(x => x.RequestedByUserId)
                .HasColumnName("requested_by_user_id")
                .IsRequired();

            // approver
            b.Property(x => x.ApprovedByUserId)
                .HasColumnName("approved_by_user_id");

            b.Property(x => x.ApprovedAt)
                .HasColumnName("approved_at");

            // timestamps
            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            // RELACJE

            // Employee 1 -> N LeaveRequests
            b.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // User (requester)
            b.HasOne(x => x.RequestedByUser)
                .WithMany()
                .HasForeignKey(x => x.RequestedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User (approver)
            b.HasOne(x => x.ApprovedByUser)
                .WithMany()
                .HasForeignKey(x => x.ApprovedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // INDEKSY

            b.HasIndex(x => x.EmployeeId);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.StartDate);
            b.HasIndex(x => new { x.EmployeeId, x.StartDate });

            // CONSTRAINTY BIZNESOWE

            b.HasCheckConstraint(
                "ck_leave_dates",
                "end_date >= start_date"
            );
        }
    }
}
