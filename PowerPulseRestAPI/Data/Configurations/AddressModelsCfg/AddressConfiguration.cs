using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.AddressModels;

namespace PowerPulseRestAPI.Data.Configurations.AddressModelsCfg
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> b)
        {
            b.ToTable("addresses");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.Country)
                .HasColumnName("country")
                .IsRequired()
                .HasMaxLength(100);

            b.Property(x => x.PostalCode)
                .HasColumnName("postal_code")
                .IsRequired()
                .HasMaxLength(20);

            b.Property(x => x.City)
                .HasColumnName("city")
                .IsRequired()
                .HasMaxLength(150);

            b.Property(x => x.Street)
                .HasColumnName("street")
                .IsRequired()
                .HasMaxLength(200);

            b.Property(x => x.BuildingNumber)
                .HasColumnName("building_number")
                .IsRequired()
                .HasMaxLength(30);

            b.Property(x => x.ApartmentNumber)
                .HasColumnName("apartment_number")
                .HasMaxLength(30);

            b.Property(x => x.FullText)
                .HasColumnName("full_text")
                .HasMaxLength(500);

            b.Property(x => x.AddressType)
                .HasColumnName("address_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.HasIndex(x => new { x.Country, x.PostalCode, x.City });
            b.HasIndex(x => x.AddressType);

            b.HasCheckConstraint("ck_address_country_not_empty", "country <> ''");
            b.HasCheckConstraint("ck_address_city_not_empty", "city <> ''");
            b.HasCheckConstraint("ck_address_street_not_empty", "street <> ''");
            b.HasCheckConstraint("ck_address_building_number_not_empty", "building_number <> ''");
            b.HasCheckConstraint("ck_address_updated_at", "updated_at >= created_at");
        }
    }
}