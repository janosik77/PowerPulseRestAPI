using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;

public class CalculatorConfiguration : IEntityTypeConfiguration<Calculator>
{
    public void Configure(EntityTypeBuilder<Calculator> b)
    {
        b.ToTable("calculators");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.Code)
            .HasColumnName("code")
            .IsRequired()
            .HasMaxLength(80);

        b.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        b.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(2000);

        b.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        b.HasIndex(x => x.Code).IsUnique();
        b.HasIndex(x => x.IsActive);

        b.HasCheckConstraint("ck_calculator_code_not_empty", "code <> ''");
    }
}
