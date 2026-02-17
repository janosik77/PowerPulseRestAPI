using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.CustomerModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class CustomerContactConfiguration
    {
        public void Configure(EntityTypeBuilder<CustomerContact> b)
        {
            b.ToTable("customer_contacts");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();

            b.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(100);

            b.Property(x => x.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(100);

            b.Property(x => x.RoleTitle)
                .HasColumnName("role_title")
                .HasMaxLength(150);

            b.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(320);

            b.Property(x => x.Phone)
                .HasColumnName("phone")
                .HasMaxLength(30);

            b.Property(x => x.IsPrimary)
                .HasColumnName("is_primary")
                .IsRequired();

            b.Property(x => x.Note)
                .HasColumnName("note")
                .HasMaxLength(4000);

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            // Relacja: Customer 1 -> N CustomerContacts
            b.HasOne(x => x.Customer)
                .WithMany(c => c.Contacts)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // bezpieczne przy soft delete customerów

            // Indeksy
            b.HasIndex(x => x.CustomerId);
            b.HasIndex(x => new { x.CustomerId, x.IsPrimary });
            b.HasIndex(x => x.Email);

            // (opcjonalnie) jedno "primary" na customer:
            b.HasIndex(x => x.CustomerId).IsUnique().HasFilter("is_primary = true");
        }
    }
}
