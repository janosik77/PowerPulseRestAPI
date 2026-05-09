using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;

public class KnowledgeArticleReadConfiguration : IEntityTypeConfiguration<KnowledgeArticleRead>
{
    public void Configure(EntityTypeBuilder<KnowledgeArticleRead> b)
    {
        b.ToTable("knowledge_article_reads");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ArticleId).HasColumnName("article_id").IsRequired();
        b.Property(x => x.UserId).HasColumnName("user_id").IsRequired();

        b.Property(x => x.ReadAt).HasColumnName("read_at").IsRequired();
        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasOne(x => x.Article)
            .WithMany(a => a.Reads)
            .HasForeignKey(x => x.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.User)
            .WithMany(u => u.KnowledgeReads)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.ArticleId);
        b.HasIndex(x => x.UserId);
        b.HasIndex(x => x.ReadAt);
    }
}
