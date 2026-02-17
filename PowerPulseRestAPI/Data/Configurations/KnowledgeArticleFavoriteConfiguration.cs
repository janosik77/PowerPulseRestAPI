using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;

public class KnowledgeArticleFavoriteConfiguration : IEntityTypeConfiguration<KnowledgeArticleFavorite>
{
    public void Configure(EntityTypeBuilder<KnowledgeArticleFavorite> b)
    {
        b.ToTable("knowledge_article_favorites");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.ArticleId).HasColumnName("article_id").IsRequired();
        b.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasOne(x => x.Article)
            .WithMany(a => a.Favorites)
            .HasForeignKey(x => x.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.User)
            .WithMany(u => u.KnowledgeFavorites)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => new { x.ArticleId, x.UserId }).IsUnique();
        b.HasIndex(x => x.UserId);
    }
}
