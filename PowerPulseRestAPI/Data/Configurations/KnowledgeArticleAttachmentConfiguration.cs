using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;

public class KnowledgeArticleAttachmentConfiguration : IEntityTypeConfiguration<KnowledgeArticleAttachment>
{
    public void Configure(EntityTypeBuilder<KnowledgeArticleAttachment> b)
    {
        b.ToTable("knowledge_article_attachments");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ArticleId).HasColumnName("article_id").IsRequired();

        b.Property(x => x.Url)
            .HasColumnName("url")
            .IsRequired()
            .HasMaxLength(2048);

        b.Property(x => x.Caption)
            .HasColumnName("caption")
            .HasMaxLength(500);

        b.Property(x => x.AttachmentType)
            .HasColumnName("attachment_type")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        b.Property(x => x.SortOrder)
            .HasColumnName("sort_order")
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        b.HasOne(x => x.Article)
            .WithMany(a => a.Attachments)
            .HasForeignKey(x => x.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.ArticleId);
        b.HasIndex(x => new { x.ArticleId, x.SortOrder });

        b.HasCheckConstraint("ck_knowledge_attachment_sort_nonneg", "sort_order >= 0");
    }
}
