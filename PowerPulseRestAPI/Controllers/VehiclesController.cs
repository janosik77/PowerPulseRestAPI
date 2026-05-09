using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.VehicleDto.Requests;
using PowerPulseRestAPI.Services.VehiclesS;
using System.Security.Claims;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateVehicleDto dto,
            CancellationToken cancellationToken)
        {
            var created = await _vehicleService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken)
        {
            var result = await _vehicleService.GetListAsync(cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(
            long id,
            CancellationToken cancellationToken)
        {
            var result = await _vehicleService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("employee")]
        public async Task<IActionResult> GetByEmployee(
            CancellationToken cancellationToken)
        {
            var employeeIdValue = User.FindFirst("employeeId")?.Value;

            if (!long.TryParse(employeeIdValue, out var employeeId))
            {
                return Unauthorized("EmployeeId claim is missing or invalid.");
            }

            var result = await _vehicleService.GetByEmployeeAsync(employeeId, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] UpdateVehicleDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _vehicleService.UpdateAsync(id, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("{id:long}/assign")]
        public async Task<IActionResult> Assign(
            long id,
            [FromBody] CreateVehicleAssignmentDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _vehicleService.AssignAsync(id, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("{id:long}/return")]
        public async Task<IActionResult> Return(
            long id,
            [FromBody] CreateVehicleAssignmentDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _vehicleService.ReturnAsync(id, dto.Note, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpPost("{id:long}/issues")]
        public async Task<IActionResult> CreateIssue(
            long id,
            [FromBody] CreateVehicleIssueDto dto,
            CancellationToken cancellationToken)
        {
            var reportedByUserId = long.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            //var reportedByUserId = long.Parse(User.Claims.First(c => c.Type == "userId").Value);
            var created = await _vehicleService.CreateIssueAsync(id, dto, reportedByUserId, cancellationToken);
            return Ok(created);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{vehicleId:long}/issues/{issueId:long}")]
        public async Task<IActionResult> UpdateIssue(
            long vehicleId,
            long issueId,
            [FromBody] UpdateVehicleIssueDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _vehicleService.UpdateIssueAsync(vehicleId, issueId, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{vehicleId:long}/issues/{issueId:long}")]
        public async Task<IActionResult> DeleteIssue(
            long vehicleId,
            long issueId,
            CancellationToken cancellationToken)
        {
            await _vehicleService.DeleteIssueAsync(vehicleId, issueId, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken cancellationToken)
        {
            await _vehicleService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}