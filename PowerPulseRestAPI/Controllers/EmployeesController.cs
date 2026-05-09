using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.EmployeeDto.Requests;
using PowerPulseRestAPI.Services.Employees;
using System.Security.Claims;


namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateEmployeeDto dto,
            CancellationToken cancellationToken)
        {
            var created = await _employeeService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetPrivateById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetListAsync(cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "USER")]
        [HttpGet("{id:long}/public")]
        public async Task<IActionResult> GetPublicById(
            long id,
            CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetPublicByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("select-options")]
        public async Task<IActionResult> GetSelectOptions(CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetSelectOptionsAsync(cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetPrivateById(
            long id,
            CancellationToken cancellationToken)
        {

            if (!CanAccessEmployee(id))
                return Forbid();

            var result = await _employeeService.GetPrivateByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] UpdateEmployeeDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _employeeService.UpdateAsync(id, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken cancellationToken)
        {
            await _employeeService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        private bool CanAccessEmployee(long requestedEmployeeId)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == "ADMIN")
                return true;

            var employeeIdClaim = User.FindFirst("employeeId")?.Value;

            if (!long.TryParse(employeeIdClaim, out var loggedEmployeeId))
                return false;

            return loggedEmployeeId == requestedEmployeeId;
        }
    }
}
