using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.DTO.ToolDto.Requests;
using PowerPulseRestAPI.Services.ToolsS;
using System.Security.Claims;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/tools")]
    public class ToolsController : ControllerBase
    {
        private readonly IToolService _toolService;

        public ToolsController(IToolService toolService)
        {
            _toolService = toolService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateToolDto dto,
            CancellationToken cancellationToken)
        {
            var createdByUserId = GetCurrentUserId();
            var created = await _toolService.CreateAsync(dto, createdByUserId, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken)
        {
            var result = await _toolService.GetListAsync(cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(
            long id,
            CancellationToken cancellationToken)
        {
            var result = await _toolService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] UpdateToolDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _toolService.UpdateAsync(id, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpPost("{id:long}/assign")]
        public async Task<IActionResult> Assign(
            long id,
            [FromBody] AssignToolDto dto,
            CancellationToken cancellationToken)
        {
            var createdByUserId = GetCurrentUserId();
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeId = GetCurrentEmployeeId();

            var updated = await _toolService.AssignAsync(role, employeeId, id, dto, createdByUserId, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpPost("{id:long}/return")]
        public async Task<IActionResult> Return(
            long id,
            [FromBody] ReturnToolDto dto,
            CancellationToken cancellationToken)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var employeeId = GetCurrentEmployeeId();

            var updated = await _toolService.ReturnAsync(role, employeeId, id, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken cancellationToken)
        {
            await _toolService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        private long GetCurrentEmployeeId()
        {
            var employeeIdClaim = User.FindFirst("employeeId")?.Value;

            if (!long.TryParse(employeeIdClaim, out var employeeId))
            {
                throw new UnauthorizedAccessException("Employee id claim is missing or invalid.");
            }

            return employeeId;
        }

        private long GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
            }

            return userId;
        }
    }
}