using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.CustomerModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class CustomerConfiguration
    {
        public void Configure(EntityTypeBuilder<Customer> b)
        {
            b.ToTable("customers");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.CustomerType)
                .HasColumnName("customer_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(300);

            b.Property(x => x.TaxId)
                .HasColumnName("tax_id")
                .HasMaxLength(50);

            b.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(320); // standardowo max dla email

            b.Property(x => x.Phone)
                .HasColumnName("phone")
                .HasMaxLength(30);

            b.Property(x => x.Note)
                .HasColumnName("note")
                .HasMaxLength(4000);

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            // INDEKSY
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.CustomerType);
            b.HasIndex(x => x.Name);

            b.HasIndex(x => x.TaxId).IsUnique().HasFilter("[tax_id] IS NOT NULL");
        }
    }
}
