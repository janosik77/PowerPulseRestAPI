using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;

public class KnowledgeArticleConfiguration : IEntityTypeConfiguration<KnowledgeArticle>
{
    public void Configure(EntityTypeBuilder<KnowledgeArticle> b)
    {
        b.ToTable("knowledge_articles");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.CategoryId).HasColumnName("category_id");

        b.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(300);

        b.Property(x => x.Content)
            .HasColumnName("content")
            .IsRequired()
            .HasMaxLength(20000);

        b.Property(x => x.ArticleType)
            .HasColumnName("article_type")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.SeverityTag)
            .HasColumnName("severity_tag")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.Version)
            .HasColumnName("version")
            .HasMaxLength(50);

        b.Property(x => x.PublishedAt)
            .HasColumnName("published_at");

        b.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id").IsRequired();
        b.Property(x => x.UpdatedByUserId).HasColumnName("updated_by_user_id").IsRequired();

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        // Relacje (dwukierunkowe dla KB; usery też dwukierunkowo)
        b.HasOne(x => x.Category)
            .WithMany(c => c.Articles)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasOne(x => x.CreatedByUser)
            .WithMany(u => u.KnowledgeArticlesCreated)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.UpdatedByUser)
            .WithMany(u => u.KnowledgeArticlesUpdated)
            .HasForeignKey(x => x.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indeksy
        b.HasIndex(x => x.CategoryId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.SeverityTag);
        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => x.PublishedAt);
        b.HasIndex(x => new { x.Status, x.PublishedAt });

        // CONSTRAINT: PublishedAt tylko gdy Status=PUBLISHED (lub odwrotnie)
        // Dostosuj literal 'PUBLISHED' do Twojego enum->string.
        b.HasCheckConstraint(
            "ck_knowledge_article_publish_consistency",
            "(status <> 'PUBLISHED' AND published_at IS NULL) OR (status = 'PUBLISHED' AND published_at IS NOT NULL)"
        );
    }
}
