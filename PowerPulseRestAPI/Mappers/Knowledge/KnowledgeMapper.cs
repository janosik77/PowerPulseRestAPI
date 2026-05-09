using PowerPulseRestAPI.Data.Models.KnowledgeModels;
using PowerPulseRestAPI.DTO.KnowlegeDto.Helpers;
using PowerPulseRestAPI.DTO.KnowlegeDto.Responses;

namespace PowerPulseRestAPI.Mappers.Knowledge
{
    public static class KnowledgeMapper
    {
        public static KnowledgeArticleListItemDto ToListItemDto(
            this KnowledgeArticle article,
            long? currentUserId = null)
        {
            return new KnowledgeArticleListItemDto
            {
                Id = article.Id,
                Title = article.Title,
                Status = article.Status,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
                Category = article.Category == null
                    ? null
                    : new KnowledgeCategoryDto
                    {
                        Id = article.Category.Id,
                        Name = article.Category.Name
                    },
                CreatedByUser = article.CreatedByUser == null
                    ? null
                    : new KnowledgeUserShortDto
                    {
                        Id = article.CreatedByUser.Id,
                        FullName = BuildUserFullName(article.CreatedByUser)
                    },
                AttachmentsCount = article.Attachments.Count,
                FavoritesCount = article.Favorites.Count,
                ReadsCount = article.Reads.Count,
                IsFavorite = currentUserId.HasValue && article.Favorites.Any(x => x.UserId == currentUserId.Value),
                IsRead = currentUserId.HasValue && article.Reads.Any(x => x.UserId == currentUserId.Value)
            };
        }

        public static KnowledgeArticleDetailsDto ToDetailsDto(
            this KnowledgeArticle article,
            long? currentUserId = null)
        {
            return new KnowledgeArticleDetailsDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Status = article.Status,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
                Category = article.Category == null
                    ? null
                    : new KnowledgeCategoryDto
                    {
                        Id = article.Category.Id,
                        Name = article.Category.Name
                    },
                CreatedByUser = article.CreatedByUser == null
                    ? null
                    : new KnowledgeUserShortDto
                    {
                        Id = article.CreatedByUser.Id,
                        FullName = BuildUserFullName(article.CreatedByUser)
                    },
                Attachments = article.Attachments
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Id)
                    .Select(x => new KnowledgeArticleAttachmentDto
                    {
                        Id = x.Id,
                        Url = x.Url,
                        Caption = x.Caption,
                        AttachmentType = x.AttachmentType,
                        SortOrder = x.SortOrder,
                        CreatedAt = x.CreatedAt
                    })
                    .ToList(),
                FavoritesCount = article.Favorites.Count,
                ReadsCount = article.Reads.Count,
                IsFavorite = currentUserId.HasValue && article.Favorites.Any(x => x.UserId == currentUserId.Value),
                IsRead = currentUserId.HasValue && article.Reads.Any(x => x.UserId == currentUserId.Value)
            };
        }

        private static string BuildUserFullName(Data.Models.UsersModels.User user)
        {
            if (user.Person == null)
            {
                return user.Login;
            }

            return $"{user.Person.FirstName} {user.Person.LastName}";
        }
    }
}