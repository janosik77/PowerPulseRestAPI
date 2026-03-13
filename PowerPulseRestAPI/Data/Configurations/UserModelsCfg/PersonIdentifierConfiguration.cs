
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Configurations.UserModelsCfg;

public class PersonIdentifierConfiguration : IEntityTypeConfiguration<PersonIdentifier>
{
    public void Configure(EntityTypeBuilder<PersonIdentifier> b)
    {
        b.ToTable("person_identifiers");

        // PK
        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .HasColumnName("id");

        // FK
        b.Property(x => x.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        b.Property(x => x.Type)
            .HasColumnName("type")
            .IsRequired()
            .HasConversion<string>()      // zapis jako tekst zamiast int
            .HasMaxLength(30);

        b.Property(x => x.ValueEncrypted)
            .HasColumnName("value_encrypted")
            .IsRequired()
            .HasMaxLength(500);

        b.Property(x => x.Last4)
            .HasColumnName("last4")
            .HasMaxLength(10);

        b.Property(x => x.Country)
            .HasColumnName("country")
            .HasMaxLength(10);

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        b.HasOne(x => x.Person)
            .WithMany(p => p.Identifiers)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => new { x.PersonId, x.Type })
            .IsUnique();

        b.HasIndex(x => x.PersonId);
    }
}
