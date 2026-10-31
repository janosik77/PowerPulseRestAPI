using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.DictionaryModels;

namespace PowerPulseRestAPI.Data.Configurations.DictionaryCfg;

public class DictionaryConfiguration : IEntityTypeConfiguration<Dictionary>
{
    public void Configure(EntityTypeBuilder<Dictionary> builder)
    {
        builder.ToTable("dictionary");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Key)
            .HasColumnName("key")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnName("value")
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(x => x.Updated)
            .HasColumnName("updated")
            .IsRequired();
        
        builder.Property(x => x.Created)
            .HasColumnName("created")
            .IsRequired();

        builder.HasIndex(x => x.Key)
            .IsUnique();
    }
}