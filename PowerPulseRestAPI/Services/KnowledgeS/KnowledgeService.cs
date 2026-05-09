using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;
using PowerPulseRestAPI.DTO.KnowlegeDto.Requests;
using PowerPulseRestAPI.DTO.KnowlegeDto.Responses;
using PowerPulseRestAPI.Mappers.Knowledge;
using PowerPulseRestAPI.Services.KnowledgeS;
using System;

namespace PowerPulseRestAPI.Services
{
    public class KnowledgeService : IKnowledgeService
    {
        private readonly PowerPulseContext _dbContext;

        public KnowledgeService(PowerPulseContext db)
        {
            _dbContext = db;
        }

        public async Task<long> CreateCategoryAsync(
            CreateKnowledgeCategoryDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var exists = await _dbContext.KnowledgeCategories
                .AnyAsync(x => x.Name == dto.Name, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException($"Knowledge category '{dto.Name}' already exists.");
            }

            var category = new KnowledgeCategory
            {
                Name = dto.Name,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            _dbContext.KnowledgeCategories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return category.Id;
        }

        public async Task UpdateCategoryAsync(
            long id,
            UpdateKnowledgeCategoryDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var category = await _dbContext.KnowledgeCategories
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (category is null)
            {
                throw new KeyNotFoundException($"Knowledge category with id '{id}' was not found.");
            }

            var exists = await _dbContext.KnowledgeCategories
                .AnyAsync(x => x.Id != id && x.Name == dto.Name, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException($"Knowledge category '{dto.Name}' already exists.");
            }

            category.Name = dto.Name;
            category.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<KnowledgeArticleDetailsDto> CreateArticleAsync(
            CreateKnowledgeArticleDto dto,
            long createdByUserId,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.CategoryId.HasValue)
            {
                var categoryExists = await _dbContext.KnowledgeCategories
                    .AnyAsync(x => x.Id == dto.CategoryId.Value, cancellationToken);

                if (!categoryExists)
                {
                    throw new KeyNotFoundException($"Knowledge category with id '{dto.CategoryId.Value}' was not found.");
                }
            }

            var userExists = await _dbContext.Users
                .AnyAsync(x => x.Id == createdByUserId, cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException($"User with id '{createdByUserId}' was not found.");
            }

            var now = DateTimeOffset.UtcNow;

            var article = new KnowledgeArticle
            {
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Content = dto.Content,
                Status = dto.Status,
                CreatedByUserId = createdByUserId,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false,
            };

            _dbContext.KnowledgeArticles.Add(article);
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (dto.Attachments.Count > 0)
            {
                var attachments = dto.Attachments.Select(x => new KnowledgeArticleAttachment
                {
                    ArticleId = article.Id,
                    Url = x.Url,
                    Caption = x.Caption,
                    AttachmentType = x.AttachmentType,
                    SortOrder = x.SortOrder,
                    CreatedAt = now
                });

                _dbContext.KnowledgeArticleAttachments.AddRange(attachments);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            var created = await GetArticleQuery()
                .FirstAsync(x => x.Id == article.Id, cancellationToken);

            return created.ToDetailsDto(createdByUserId);
        }

        public async Task<IReadOnlyList<KnowledgeArticleListItemDto>> GetArticlesAsync(
            long? currentUserId = null,
            CancellationToken cancellationToken = default)
        {
            var articles = await GetArticleQuery()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.UpdatedAt)
                .ToListAsync(cancellationToken);

            return articles
                .Select(x => x.ToListItemDto(currentUserId))
                .ToList();
        }

        public async Task<KnowledgeArticleDetailsDto> GetArticleByIdAsync(
            long id,
            long? currentUserId = null,
            CancellationToken cancellationToken = default)
        {
            var article = await GetArticleQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (article is null)
            {
                throw new KeyNotFoundException($"Knowledge article with id '{id}' was not found.");
            }

            return article.ToDetailsDto(currentUserId);
        }

        public async Task<KnowledgeArticleDetailsDto> UpdateArticleAsync(
            long id,
            UpdateKnowledgeArticleDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var article = await _dbContext.KnowledgeArticles
                .Include(x => x.Attachments)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (article is null)
            {
                throw new KeyNotFoundException($"Knowledge article with id '{id}' was not found.");
            }

            if (dto.CategoryId.HasValue)
            {
                var categoryExists = await _dbContext.KnowledgeCategories
                    .AnyAsync(x => x.Id == dto.CategoryId.Value, cancellationToken);

                if (!categoryExists)
                {
                    throw new KeyNotFoundException($"Knowledge category with id '{dto.CategoryId.Value}' was not found.");
                }
            }

            article.CategoryId = dto.CategoryId;
            article.Title = dto.Title;
            article.Content = dto.Content;
            article.Status = dto.Status;
            article.UpdatedAt = DateTimeOffset.UtcNow;

            var now = DateTimeOffset.UtcNow;
            var incomingIds = dto.Attachments
                .Where(x => x.Id.HasValue)
                .Select(x => x.Id!.Value)
                .ToHashSet();

            var toRemove = article.Attachments
                .Where(x => !incomingIds.Contains(x.Id))
                .ToList();

            if (toRemove.Count > 0)
            {
                _dbContext.KnowledgeArticleAttachments.RemoveRange(toRemove);
            }

            foreach (var attachmentDto in dto.Attachments)
            {
                if (attachmentDto.Id.HasValue)
                {
                    var existing = article.Attachments.FirstOrDefault(x => x.Id == attachmentDto.Id.Value);
                    if (existing is null)
                    {
                        throw new InvalidOperationException($"Attachment with id '{attachmentDto.Id.Value}' does not belong to this article.");
                    }

                    existing.Url = attachmentDto.Url;
                    existing.Caption = attachmentDto.Caption;
                    existing.AttachmentType = attachmentDto.AttachmentType;
                    existing.SortOrder = attachmentDto.SortOrder;
                }
                else
                {
                    article.Attachments.Add(new KnowledgeArticleAttachment
                    {
                        ArticleId = article.Id,
                        Url = attachmentDto.Url,
                        Caption = attachmentDto.Caption,
                        AttachmentType = attachmentDto.AttachmentType,
                        SortOrder = attachmentDto.SortOrder,
                        CreatedAt = now
                    });
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            var updated = await GetArticleQuery()
                .FirstAsync(x => x.Id == id, cancellationToken);

            return updated.ToDetailsDto();
        }

        public async Task DeleteArticleAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var article = await _dbContext.KnowledgeArticles
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (article is null)
            {
                throw new KeyNotFoundException($"Knowledge article with id '{id}' was not found.");
            }

            article.IsDeleted = true;
            article.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ToggleFavoriteAsync(
            long articleId,
            long userId,
            CancellationToken cancellationToken = default)
        {
            var articleExists = await _dbContext.KnowledgeArticles
                .AnyAsync(x => x.Id == articleId, cancellationToken);

            if (!articleExists)
            {
                throw new KeyNotFoundException($"Knowledge article with id '{articleId}' was not found.");
            }

            var userExists = await _dbContext.Users
                .AnyAsync(x => x.Id == userId, cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException($"User with id '{userId}' was not found.");
            }

            var existing = await _dbContext.KnowledgeArticleFavorites
                .FirstOrDefaultAsync(x => x.ArticleId == articleId && x.UserId == userId, cancellationToken);

            if (existing is null)
            {
                _dbContext.KnowledgeArticleFavorites.Add(new KnowledgeArticleFavorite
                {
                    ArticleId = articleId,
                    UserId = userId,
                    CreatedAt = DateTimeOffset.UtcNow
                });
            }
            else
            {
                _dbContext.KnowledgeArticleFavorites.Remove(existing);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkAsReadAsync(
            long articleId,
            long userId,
            CancellationToken cancellationToken = default)
        {
            var articleExists = await _dbContext.KnowledgeArticles
                .AnyAsync(x => x.Id == articleId, cancellationToken);

            if (!articleExists)
            {
                throw new KeyNotFoundException($"Knowledge article with id '{articleId}' was not found.");
            }

            var userExists = await _dbContext.Users
                .AnyAsync(x => x.Id == userId, cancellationToken);

            if (!userExists)
            {
                throw new KeyNotFoundException($"User with id '{userId}' was not found.");
            }

            var existing = await _dbContext.KnowledgeArticleReads
                .FirstOrDefaultAsync(x => x.ArticleId == articleId && x.UserId == userId, cancellationToken);

            var now = DateTimeOffset.UtcNow;

            _dbContext.KnowledgeArticleReads.Add(new KnowledgeArticleRead
            {
                ArticleId = articleId,
                UserId = userId,
                ReadAt = now,
                CreatedAt = now
            });

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<KnowledgeArticle> GetArticleQuery()
        {
            return _dbContext.KnowledgeArticles
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.CreatedByUser)
                    .ThenInclude(x => x.Person)
                .Include(x => x.Attachments)
                .Include(x => x.Favorites)
                .Include(x => x.Reads);
        }

        public async Task<IReadOnlyList<KnowledgeCategorySelectOptionsDto>> GetKnowledgeCategoriesAsync(CancellationToken cancellationToken = default)
        {
             var categories = await _dbContext.KnowledgeCategories
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new KnowledgeCategorySelectOptionsDto
                {
                    Value = x.Id,
                    Label = x.Name
                })
                .ToListAsync(cancellationToken);

            return categories;
        }
    }
}