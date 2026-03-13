using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.CustomerModels;

namespace PowerPulseRestAPI.Data.Configurations.CustomerModelsCfg;

public class CustomerPersonConfiguration : IEntityTypeConfiguration<CustomerPerson>
{
    public void Configure(EntityTypeBuilder<CustomerPerson> b)
    {
        b.ToTable("customer_person");

        b.HasKey(x => new { x.CustomerId, x.PersonId });

        b.Property(x => x.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        b.Property(x => x.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        b.Property(x => x.ContactRole)
            .HasColumnName("contact_role")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.IsPrimary)
            .HasColumnName("is_primary")
            .IsRequired();

        b.HasOne(x => x.Customer)
            .WithMany(x => x.Contacts)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Person)
            .WithMany(x => x.CustomerLinks)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.PersonId);

        b.HasIndex(x => new { x.CustomerId, x.IsPrimary });

        b.HasIndex(x => x.CustomerId)
            .IsUnique()
            .HasFilter("[is_primary] = 1");
    }
}