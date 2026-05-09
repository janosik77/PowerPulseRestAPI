using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.KnowlegeDto.Requests;
using PowerPulseRestAPI.Services.KnowledgeS;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/knowledge")]
    public class KnowledgeController : ControllerBase
    {
        private readonly IKnowledgeService _knowledgeService;

        public KnowledgeController(IKnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CreateKnowledgeCategoryDto dto,
            CancellationToken cancellationToken)
        {
            var id = await _knowledgeService.CreateCategoryAsync(dto, cancellationToken);
            return Ok(new { id });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("categories/{id:long}")]
        public async Task<IActionResult> UpdateCategory(
            long id,
            [FromBody] UpdateKnowledgeCategoryDto dto,
            CancellationToken cancellationToken)
        {
            await _knowledgeService.UpdateCategoryAsync(id, dto, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("categories/select-options")]
        public async Task<IActionResult> GetCategorySelectOptionsAsync(CancellationToken cancellationToken)
        {
            var result = await _knowledgeService.GetKnowledgeCategoriesAsync();
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("articles")]
        public async Task<IActionResult> CreateArticle(
            [FromBody] CreateKnowledgeArticleDto dto,
            [FromQuery] long createdByUserId,
            CancellationToken cancellationToken)
        {
            var created = await _knowledgeService.CreateArticleAsync(dto, createdByUserId, cancellationToken);
            return CreatedAtAction(nameof(GetArticleById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("articles")]
        public async Task<IActionResult> GetArticles(
            [FromQuery] long? currentUserId,
            CancellationToken cancellationToken)
        {
            var result = await _knowledgeService.GetArticlesAsync(currentUserId, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("articles/{id:long}")]
        public async Task<IActionResult> GetArticleById(
            long id,
            [FromQuery] long? currentUserId,
            CancellationToken cancellationToken)
        {
            var result = await _knowledgeService.GetArticleByIdAsync(id, currentUserId, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("articles/{id:long}")]
        public async Task<IActionResult> UpdateArticle(
            long id,
            [FromBody] UpdateKnowledgeArticleDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _knowledgeService.UpdateArticleAsync(id, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("articles/{id:long}")]
        public async Task<IActionResult> DeleteArticle(
            long id,
            CancellationToken cancellationToken)
        {
            await _knowledgeService.DeleteArticleAsync(id, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpPost("articles/{id:long}/favorite")]
        public async Task<IActionResult> ToggleFavorite(
            long id,
            [FromQuery] long userId,
            CancellationToken cancellationToken)
        {
            await _knowledgeService.ToggleFavoriteAsync(id, userId, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpPost("articles/{id:long}/read")]
        public async Task<IActionResult> MarkAsRead(
            long id,
            [FromQuery] long userId,
            CancellationToken cancellationToken)
        {
            await _knowledgeService.MarkAsReadAsync(id, userId, cancellationToken);
            return NoContent();
        }
    }
}
