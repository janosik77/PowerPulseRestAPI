using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.CustomerModels;


namespace PowerPulseRestAPI.Data.Configurations.CustomerModelsCfg
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> b)
        {
            b.ToTable("customers");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.CompanyName)
                .HasColumnName("company_name")
                .IsRequired()
                .HasMaxLength(300);

            b.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(50);

            b.Property(x => x.AvatarUrl)
            .HasColumnName("avatar_url")
            .HasMaxLength(2048);

            b.Property(x => x.TaxId)
                .HasColumnName("tax_id")
                .HasMaxLength(50);

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.CompanyName);
            b.HasIndex(x => x.TaxId)
                .IsUnique()
                .HasFilter("[tax_id] IS NOT NULL");

            b.HasCheckConstraint("ck_customer_company_name_not_empty", "company_name <> ''");
            b.HasCheckConstraint("ck_customer_updated_at", "updated_at >= created_at");
        }
    }
}
