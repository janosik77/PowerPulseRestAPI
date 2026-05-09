using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.ToolDto.Requests;
using PowerPulseRestAPI.Services.ToolsS;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/tool-categories")]
    public class ToolCategoriesController : ControllerBase
    {
        private readonly IToolCategoryService _toolCategoryService;

        public ToolCategoriesController(IToolCategoryService toolCategoryService)
        {
            _toolCategoryService = toolCategoryService;
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("select-options")]
        public async Task<IActionResult> GetSelectOptions(
            CancellationToken cancellationToken)
        {
            var result = await _toolCategoryService.GetSelectOptionsAsync(cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateToolCategoryDto dto,
            CancellationToken cancellationToken)
        {
            var id = await _toolCategoryService.CreateAsync(dto, cancellationToken);
            return Ok(new { id });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken cancellationToken)
        {
            await _toolCategoryService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
