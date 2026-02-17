using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class ProjectCustomerContactConfiguration
    {
        public void Configure(EntityTypeBuilder<ProjectCustomerContact> b)
        {
            b.ToTable("project_customer_contacts");

            // PK złożony (zgodnie z opisem)
            b.HasKey(x => new { x.ProjectId, x.CustomerContactId, x.ContactRole });

            b.Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            b.Property(x => x.CustomerContactId)
                .HasColumnName("customer_contact_id")
                .IsRequired();

            b.Property(x => x.ContactRole)
                .HasColumnName("contact_role")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            // Relacje
            b.HasOne(x => x.Project)
                .WithMany(p => p.CustomerContacts)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.CustomerContact)
                .WithMany(cc => cc.ProjectLinks) 
                .HasForeignKey(x => x.CustomerContactId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indeksy (pod szybkie lookupy)
            b.HasIndex(x => x.ProjectId);
            b.HasIndex(x => x.CustomerContactId);
            b.HasIndex(x => new { x.ProjectId, x.CustomerContactId });
        }
    }
}
