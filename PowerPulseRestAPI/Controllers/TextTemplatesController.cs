using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.TextControlDto.Requests;
using PowerPulseRestAPI.DTO.TextControlDto.Responses;
using PowerPulseRestAPI.Services.TextTemplateS;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/text-templates")]
    public class TextTemplatesController : ControllerBase
    {
        private readonly ITextTemplateService _textTemplateService;

        public TextTemplatesController(ITextTemplateService textTemplateService)
        {
            _textTemplateService = textTemplateService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateTextTemplateDto dto,
            CancellationToken cancellationToken)
        {
            var created = await _textTemplateService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken)
        {
            var result = await _textTemplateService.GetListAsync(cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(
            long id,
            CancellationToken cancellationToken)
        {
            var result = await _textTemplateService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] UpdateTextTemplateDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _textTemplateService.UpdateAsync(id, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken cancellationToken)
        {
            await _textTemplateService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{id}/is-active")]
        public async Task<ActionResult<TextTemplateDto>> SetIsActive(
            long id,
            [FromBody] SetIsActiveDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _textTemplateService.SetActiveAsync(id, dto.IsActive, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("options")]
        [ProducesResponseType(typeof(IReadOnlyList<TextTemplateOptionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<TextTemplateOptionDto>>> GetOptions(
            CancellationToken cancellationToken)
        {
            var result = await _textTemplateService.GetOptionsAsync(cancellationToken);
            return Ok(result);
        }
    }
}