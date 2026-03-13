using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.TextControlModels;

namespace PowerPulseRestAPI.Data.Configurations.TextControlCfg
{
    public class TextTemplateConfiguration : IEntityTypeConfiguration<TextTemplate>
    {
        public void Configure(EntityTypeBuilder<TextTemplate> b)
        {
            b.ToTable("text_templates");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
                .HasColumnName("id");

            b.Property(x => x.Key)
                .HasColumnName("key")
                .IsRequired()
                .HasMaxLength(200);

            b.Property(x => x.Channel)
                .HasColumnName("channel")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            b.Property(x => x.Language)
                .HasColumnName("language")
                .HasMaxLength(10);

            b.Property(x => x.TitleTemplate)
                .HasColumnName("title_template")
                .HasMaxLength(1000);

            b.Property(x => x.BodyTemplate)
                .HasColumnName("body_template")
                .IsRequired()
                .HasMaxLength(8000);

            b.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            b.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            b.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            b.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            b.HasIndex(x => new { x.Key, x.Channel, x.Language })
                .IsUnique();

            b.HasIndex(x => x.Channel);

            b.HasCheckConstraint("ck_text_template_key_not_empty", "[key] <> ''");
            b.HasCheckConstraint("ck_text_template_body_not_empty", "body_template <> ''");
            b.HasCheckConstraint("ck_text_template_updated_at", "updated_at >= created_at");
        }
    }
}
