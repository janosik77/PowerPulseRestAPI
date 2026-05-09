using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Configurations.UserModelsCfg;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("users");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        b.HasIndex(x => x.PersonId)
            .IsUnique();

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

        b.Property(x => x.RoleId)
            .HasColumnName("role_id")
            .IsRequired();

        b.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired()
            .HasDefaultValue(false);

        b.HasIndex(x => x.RoleId);

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

        b.HasOne(x => x.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(u => u.Person)
            .WithOne(p => p.User)
            .HasForeignKey<User>(u => u.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasCheckConstraint("ck_user_email_not_empty", "email <> ''");
        b.HasCheckConstraint("ck_user_login_not_empty", "login <> ''");
        b.HasCheckConstraint("ck_user_password_hash_not_empty", "password_hash <> ''");
    }
}
