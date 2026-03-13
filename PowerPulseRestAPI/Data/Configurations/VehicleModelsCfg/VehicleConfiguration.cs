using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.VehicleModels;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> b)
    {
        b.ToTable("vehicles");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        b.Property(x => x.PlateNumber)
            .HasColumnName("plate_number")
            .IsRequired()
            .HasMaxLength(30);

        b.Property(x => x.Vin)
            .HasColumnName("vin")
            .HasMaxLength(50);

        b.Property(x => x.Make)
            .HasColumnName("make")
            .HasMaxLength(100);

        b.Property(x => x.Url)
            .HasColumnName("url")
            .IsRequired()
            .HasMaxLength(2048);

        b.Property(x => x.Caption)
            .HasColumnName("caption")
            .HasMaxLength(500);

        b.Property(x => x.Model)
            .HasColumnName("model")
            .HasMaxLength(100);

        b.Property(x => x.Year)
            .HasColumnName("year");

        b.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.CurrentMileage)
            .HasColumnName("current_mileage");

        b.Property(x => x.LastServiceAt)
            .HasColumnName("last_service_at")
            .HasColumnType("date");

        b.Property(x => x.LastServiceMileage)
            .HasColumnName("last_service_mileage");

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        // Indeksy / unikalności
        b.HasIndex(x => x.PlateNumber).IsUnique();
        b.HasIndex(x => x.Vin)
            .IsUnique()
            .HasFilter("[vin] IS NOT NULL");
        b.HasIndex(x => x.Status);

        // Constrainty
        b.HasCheckConstraint("ck_vehicle_year_range", "year IS NULL OR (year >= 1900 AND year <= 2100)");
        b.HasCheckConstraint("ck_vehicle_mileage_nonneg", "current_mileage IS NULL OR current_mileage >= 0");
        b.HasCheckConstraint("ck_vehicle_last_service_mileage_nonneg", "last_service_mileage IS NULL OR last_service_mileage >= 0");
    }
}
