using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class ProjectCustomerConfiguration
    {
        public void Configure(EntityTypeBuilder<ProjectCustomer> b)
        {
            b.ToTable("project_customers");

            // PK złożony (tak jak w Twoim opisie)
            b.HasKey(x => new { x.ProjectId, x.CustomerId, x.RelationshipType });

            b.Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            b.Property(x => x.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();

            b.Property(x => x.RelationshipType)
                .HasColumnName("relationship_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.IsPrimaryOwner)
                .HasColumnName("is_primary_owner");

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            // Relacje
            b.HasOne(x => x.Project)
                .WithMany(p => p.ProjectCustomers)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Customer)
                .WithMany(c => c.ProjectCustomers) 
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indeksy
            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => x.CustomerId);
            b.HasIndex(x => new { x.ProjectId, x.CustomerId });

        }
    }
}
