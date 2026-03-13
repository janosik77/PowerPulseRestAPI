using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.ToolDto.Request;
using PowerPulseRestAPI.DTO.ToolDto.Response;
using PowerPulseRestAPI.Services.Tools;
using System.Security.Claims;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/tools")]
    public sealed class ToolsController : ControllerBase
    {
        private readonly IToolService _svc;

        public ToolsController(IToolService svc)
        {
            _svc = svc;
        }

        private long GetUserIdOrThrow()
        {
            var raw =
                User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue("sub") ??
                User.FindFirstValue("userId");

            if (string.IsNullOrWhiteSpace(raw) || !long.TryParse(raw, out var id))
                throw new UnauthorizedAccessException("Missing user id claim.");

            return id;
        }

        private bool IsManager() => User.IsInRole("Manager");

        [HttpGet]
        [Authorize]
        public Task<IReadOnlyList<ToolListItemDto>> GetList([FromQuery] ToolListQuery query, CancellationToken ct)
            => _svc.GetListAsync(query, ct);

        [HttpGet("holdings")]
        [Authorize]
        public Task<IReadOnlyList<ToolHoldingDto>> GetHoldings([FromQuery] ToolHoldingsQuery query, CancellationToken ct)
            => _svc.GetHoldingsAsync(query, GetUserIdOrThrow(), IsManager(), ct);

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<long>> Create([FromBody] ToolCreateRequest req, CancellationToken ct)
        {
            var id = await _svc.CreateAsync(req, GetUserIdOrThrow(), ct);
            return Ok(id);
        }

        [HttpPut("{toolId:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(long toolId, [FromBody] ToolUpdateRequest req, CancellationToken ct)
        {
            var ok = await _svc.UpdateAsync(toolId, req, GetUserIdOrThrow(), ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{toolId:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> SoftDelete(long toolId, CancellationToken ct)
        {
            var ok = await _svc.SoftDeleteAsync(toolId, GetUserIdOrThrow(), ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpPost("assign")]
        [Authorize]
        public Task<ToolOperationResultDto> Assign([FromBody] ToolAssignRequest req, CancellationToken ct)
            => _svc.AssignAsync(req, GetUserIdOrThrow(), IsManager(), ct);

        [HttpPost("{toolId:long}/return-to-storage")]
        [Authorize]
        public Task<ToolOperationResultDto> ReturnToStorage(long toolId, [FromBody] ToolReturnToStorageRequest req, CancellationToken ct)
            => _svc.ReturnToStorageAsync(toolId, req, GetUserIdOrThrow(), IsManager(), ct);

        [HttpPost("{toolId:long}/issues")]
        [Authorize]
        public async Task<ActionResult<long>> ReportIssue(long toolId, [FromBody] ToolIssueCreateRequest req, CancellationToken ct)
        {
            var id = await _svc.ReportIssueAsync(toolId, req, GetUserIdOrThrow(), ct);
            return Ok(id);
        }
    }
}
