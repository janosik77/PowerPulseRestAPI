using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> b)
    {
        b.ToTable("person");

        // PK
        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        // Columns (snake_case)
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
            .HasColumnType("date"); // ważne dla pełnej kompatybilności

        b.Property(x => x.AvatarUrl)
            .HasColumnName("avatar_url")
            .HasMaxLength(2048);

        b.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        b.HasIndex(x => x.UserId).IsUnique();

        b.HasOne(x => x.User)
            .WithOne(u => u.Person)
            .HasForeignKey<Person>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // (opcjonalnie) indeks na nazwisko do wyszukiwania
        b.HasIndex(x => x.LastName);

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .ValueGeneratedOnAdd();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate();
    }
}
