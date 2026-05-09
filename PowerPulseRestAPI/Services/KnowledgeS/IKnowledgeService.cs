using PowerPulseRestAPI.DTO.KnowlegeDto.Requests;
using PowerPulseRestAPI.DTO.KnowlegeDto.Responses;

namespace PowerPulseRestAPI.Services.KnowledgeS
{
    public interface IKnowledgeService
    {
        Task<long> CreateCategoryAsync(
           CreateKnowledgeCategoryDto dto,
           CancellationToken cancellationToken = default);

        Task UpdateCategoryAsync(
            long id,
            UpdateKnowledgeCategoryDto dto,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<KnowledgeCategorySelectOptionsDto>> GetKnowledgeCategoriesAsync(
            CancellationToken cancellationToken = default);

        Task<KnowledgeArticleDetailsDto> CreateArticleAsync(
            CreateKnowledgeArticleDto dto,
            long createdByUserId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<KnowledgeArticleListItemDto>> GetArticlesAsync(
            long? currentUserId = null,
            CancellationToken cancellationToken = default);

        Task<KnowledgeArticleDetailsDto> GetArticleByIdAsync(
            long id,
            long? currentUserId = null,
            CancellationToken cancellationToken = default);

        Task<KnowledgeArticleDetailsDto> UpdateArticleAsync(
            long id,
            UpdateKnowledgeArticleDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteArticleAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task ToggleFavoriteAsync(
            long articleId,
            long userId,
            CancellationToken cancellationToken = default);

        Task MarkAsReadAsync(
            long articleId,
            long userId,
            CancellationToken cancellationToken = default);
    }
}
