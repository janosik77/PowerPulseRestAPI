using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.AddressModels;

public class EntityAddressConfiguration : IEntityTypeConfiguration<EntityAddress>
{
    public void Configure(EntityTypeBuilder<EntityAddress> b)
    {
        b.ToTable("entity_addresses");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.AddressId).HasColumnName("address_id").IsRequired();

        b.Property(x => x.EntityType)
            .HasColumnName("entity_type")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.EntityId).HasColumnName("entity_id").IsRequired();

        b.Property(x => x.AddressType)
            .HasColumnName("address_type")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.IsPrimary).HasColumnName("is_primary").IsRequired();

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasOne(x => x.Address)
            .WithMany(a => a.EntityLinks)
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Cascade);

        // indeksy
        b.HasIndex(x => x.AddressId);
        b.HasIndex(x => new { x.EntityType, x.EntityId });
        b.HasIndex(x => new { x.EntityType, x.EntityId, x.AddressType });

        // zapobiega duplikatom tego samego linku:
        b.HasIndex(x => new { x.AddressId, x.EntityType, x.EntityId, x.AddressType }).IsUnique();

        b.HasCheckConstraint("ck_entity_address_entity_id_positive", "entity_id > 0");

        // (opcjonalnie) jedna główna dla danego typu adresu per encja (wymaga partial unique index zależny od DB)
        // b.HasIndex(x => new { x.EntityType, x.EntityId, x.AddressType })
        //  .IsUnique()
        //  .HasFilter("is_primary = true");
    }
}
