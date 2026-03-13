using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Configurations.UserModelsCfg;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> b)
    {
        b.ToTable("persons");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(100);

        b.Property(x => x.LastName)
            .HasColumnName("last_name")
            .IsRequired()
            .HasMaxLength(100);

        b.Property(x => x.Phone)
            .HasColumnName("phone")
            .HasMaxLength(30);

        b.Property(x => x.DateOfBirth)
            .HasColumnName("date_of_birth")
            .HasColumnType("date"); 

        b.Property(x => x.AvatarUrl)
            .HasColumnName("avatar_url")
            .HasMaxLength(2048);

        b.Property(x => x.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(255);

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        b.HasIndex(x => x.LastName);
        b.HasIndex(x => x.Email);

        b.HasMany(x => x.CustomerLinks)
            .WithOne(x => x.Person)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasCheckConstraint("ck_person_first_name_not_empty", "first_name <> ''");
        b.HasCheckConstraint("ck_person_last_name_not_empty", "last_name <> ''");
        b.HasCheckConstraint("ck_person_email_not_empty", "email <> ''");
        b.HasCheckConstraint("ck_person_updated_at", "updated_at >= created_at");
    }
}
