using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.BuissnesLogic.DictionaryS;
using PowerPulseRestAPI.DTO.DictionaryDto.Requests;
using PowerPulseRestAPI.DTO.DictionaryDto.Responses;

namespace PowerPulseRestAPI.Controllers;

[ApiController]
[Route("api/dictionary")]
[Authorize]
public class DictionaryController : ControllerBase
{
    private readonly IDictionaryService
        _dictionaryService;

    public DictionaryController(
        IDictionaryService dictionaryService)
    {
        _dictionaryService =
            dictionaryService;
    }

    [HttpGet]
    public async Task<
            ActionResult<IReadOnlyDictionary<string, string>>>
        GetAll(
            CancellationToken cancellationToken = default)
    {
        var result =
            await _dictionaryService
                .GetAllAsync(
                    cancellationToken);

        return Ok(result);
    }

    [HttpGet("overview")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<DictionaryOverviewDto>>
        GetOverview(
            [FromQuery] string? search = null,
            CancellationToken cancellationToken = default)
    {
        var result =
            await _dictionaryService
                .GetOverviewAsync(
                    search,
                    cancellationToken);

        return Ok(
            result);
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<DictionaryDto>>
        GetByKey(
            string key,
            CancellationToken cancellationToken = default)
    {
        var result =
            await _dictionaryService
                .GetByKeyAsync(
                    key,
                    cancellationToken);

        return Ok(
            result);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<DictionaryDto>>
        Create(
            [FromBody] CreateDictionaryDto dto,
            CancellationToken cancellationToken = default)
    {
        var result =
            await _dictionaryService
                .CreateAsync(
                    dto,
                    cancellationToken);

        return Ok(
            result);
    }

    [HttpPut("{key}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<DictionaryDto>>
        Update(
            string key,
            [FromBody] UpdateDictionaryDto dto,
            CancellationToken cancellationToken = default)
    {
        var result =
            await _dictionaryService
                .UpdateAsync(
                    key,
                    dto,
                    cancellationToken);

        return Ok(
            result);
    }

    [HttpDelete("{key}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>
        Delete(
            string key,
            CancellationToken cancellationToken = default)
    {
        await _dictionaryService
            .DeleteAsync(
                key,
                cancellationToken);

        return NoContent();
    }
}