using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.WorkSessionDto.Requests;
using PowerPulseRestAPI.BuissnesLogic.WorkSessionS;
using PowerPulseRestAPI.DTO.WorkSessionDto.Responses;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/work-sessions")]
    // [Authorize(Roles = "ADMIN,USER")]
    public class WorkSessionsController : ControllerBase
    {
        private readonly IWorkSessionService _workSessionService;

        public WorkSessionsController(IWorkSessionService workSessionService)
        {
            _workSessionService = workSessionService;
        }
        
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats(
            [FromQuery] WorkSessionFilterDto filter,
            CancellationToken cancellationToken)
        {
            var stats = await _workSessionService.GetStatsAsync(
                User,
                filter,
                cancellationToken);

            return Ok(stats);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateWorkSessionDto dto,
            CancellationToken cancellationToken)
        {
            var created = await _workSessionService.CreateAsync(User, dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery] WorkSessionFilterDto filter,
            CancellationToken cancellationToken)
        {
            var sessions = await _workSessionService.GetListAsync(
                User,
                filter,
                cancellationToken);

            return Ok(sessions);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(
            long id,
            CancellationToken cancellationToken)
        {
            var result = await _workSessionService.GetByIdAsync(User, id, cancellationToken);
            return Ok(result);
        }

        [HttpGet("employee/{employeeId:long}")]
        public async Task<IActionResult> GetByEmployeeId(
            long employeeId,
            CancellationToken cancellationToken)
        {
            var result = await _workSessionService.GetByEmployeeIdAsync(User, employeeId, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] UpdateWorkSessionDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _workSessionService.UpdateAsync(User, id, dto, cancellationToken);
            return Ok(updated);
        }

        [HttpPatch("{id:long}/close")]
        public async Task<IActionResult> Close(
            long id,
            [FromBody] CloseWorkSessionDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _workSessionService.CloseAsync(User, id, dto, cancellationToken);
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken cancellationToken)
        {
            await _workSessionService.DeleteAsync(User, id, cancellationToken);
            return NoContent();
        }
    }
}