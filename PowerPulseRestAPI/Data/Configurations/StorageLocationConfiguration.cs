using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

namespace PowerPulseRestAPI.Data.Configurations
{
    public class StorageLocationConfiguration
    {
        public void Configure(EntityTypeBuilder<StorageLocation> b)
        {
            b.ToTable("storage_locations");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.Code)
                .HasColumnName("code")
                .IsRequired()
                .HasMaxLength(50);

            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.HasIndex(x => x.Code).IsUnique();
        }
    }
}
