using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("users");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(320);

        b.Property(x => x.Login)
            .HasColumnName("login")
            .IsRequired()
            .HasMaxLength(100);

        b.Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();

        b.Property(x => x.LastPasswordUpdate).HasColumnName("last_password_update");
        b.Property(x => x.LastLoginAt).HasColumnName("last_login_at");

        b.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        b.HasIndex(x => x.Email).IsUnique();
        b.HasIndex(x => x.Login).IsUnique();

        b.HasIndex(x => x.IsActive);

        b.HasOne(u => u.Person)
            .WithOne(p => p.User)
            .HasForeignKey<Person>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasMany(u => u.Notifications)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasCheckConstraint("ck_user_email_not_empty", "email <> ''");
        b.HasCheckConstraint("ck_user_login_not_empty", "login <> ''");
        b.HasCheckConstraint("ck_user_password_hash_not_empty", "password_hash <> ''");
    }
}
