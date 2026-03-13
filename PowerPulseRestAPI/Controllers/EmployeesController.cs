using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.DTO.EmployeeDto.Requests;
using PowerPulseRestAPI.Services.Employees;
using System.Security.Claims;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public sealed class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _svc;

        public EmployeesController(IEmployeeService svc) => _svc = svc;

        // manager-only list
        // [Authorize(Roles = "MANAGER")]
        [HttpGet]
        public async Task<IActionResult> GetCards(CancellationToken ct)
            => Ok(await _svc.GetCardsAsync(ct));

        // details: public vs private zależnie od roli/self
        // // [Authorize]
        [HttpGet("{id:long}/details")]
        public async Task<IActionResult> GetDetails(long id, CancellationToken ct)
        {
            //var viewerUserId = GetUserIdOrThrow();
            //var isManager = User.IsInRole("MANAGER");

            //var targetUserId = await _svc.GetEmployeeUserIdAsync(id, ct);
            //if (targetUserId is null) return NotFound();

            //var isSelf = targetUserId.Value == viewerUserId;

            //if (isManager || isSelf)
            //{
            //    var dto = await _svc.GetPrivateDetailsAsync(id, ct);
            //    return dto is null ? NotFound() : Ok(dto);
            //}

            var pub = await _svc.GetPublicDetailsAsync(id, ct);
            return pub is null ? NotFound() : Ok(pub);
        }

        // tools: zgodnie z wymaganiem "po kliknięciu"
        // // [Authorize]
        [HttpGet("{id:long}/tools")]
        public async Task<IActionResult> GetTools(long id, CancellationToken ct)
        {
            var tools = await _svc.GetToolsAsync(id, ct);
            return tools is null ? NotFound() : Ok(tools);
        }

        // leaves: manager/self (bo to poufne)
        // [Authorize]
        [HttpGet("{id:long}/leaves")]
        public async Task<IActionResult> GetLeaves(long id, [FromQuery] DateOnly from, [FromQuery] DateOnly to, CancellationToken ct)
        {
            //var viewerUserId = GetUserIdOrThrow();
            //var isManager = User.IsInRole("MANAGER");
            //var targetUserId = await _svc.GetEmployeeUserIdAsync(id, ct);
            //if (targetUserId is null) return NotFound();

            //var isSelf = targetUserId.Value == viewerUserId;
            //if (!isManager && !isSelf) return Forbid();

            return Ok(await _svc.GetLeavesAsync(id, from, to, ct));
        }

        // [Authorize]
        [HttpPost("me/tools/{toolAssignmentId:long}/return")]
        public async Task<IActionResult> ReturnMyTool(long toolAssignmentId, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var ok = await _svc.ReturnMyToolAsync(toolAssignmentId, userId, ct);
            return ok ? NoContent() : NotFound();
        }

        // CREATE/UPDATE/TERMINATE
        //// [Authorize(Roles = "MANAGER")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateRequest req, CancellationToken ct)
        {
            var managerUserId = 1; /*GetUserIdOrThrow();*/
            try
            {
                var id = await _svc.CreateAsync(req, managerUserId, ct);
                return Created($"/api/employees/{id}", new { id });
 
            }
            catch (InvalidOperationException ex)
            {
                // email/login już istnieje
                return Conflict(new { message = ex.Message });
            }
        }

        // [Authorize(Roles = "MANAGER")]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] EmployeeUpdateRequest req, CancellationToken ct)
            => (await _svc.UpdateAsync(id, req, ct)) ? NoContent() : NotFound();

        // [Authorize(Roles = "MANAGER")]
        [HttpPost("{id:long}/terminate")]
        public async Task<IActionResult> Terminate(long id, [FromBody] EmployeeTerminateRequest req, CancellationToken ct)
            => (await _svc.TerminateAsync(id, req, ct)) ? NoContent() : NotFound();

        // VEHICLES
        // [Authorize(Roles = "MANAGER")]
        [HttpPost("{id:long}/vehicle/assign")]
        public async Task<IActionResult> AssignVehicle(long id, [FromBody] EmployeeAssignVehicleRequest req, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            return (await _svc.AssignVehicleAsync(id, req, managerUserId, ct)) ? NoContent() : NotFound();
        }

        // [Authorize(Roles = "MANAGER")]
        [HttpPost("{id:long}/vehicle/detach")]
        public async Task<IActionResult> DetachVehicle(long id, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            return (await _svc.DetachCurrentVehicleAsync(id, managerUserId, ct)) ? NoContent() : NotFound();
        }

        // TOOLS
        // [Authorize(Roles = "MANAGER")]
        [HttpPost("{id:long}/tools/assign")]
        public async Task<IActionResult> AssignTool(long id, [FromBody] EmployeeAssignToolRequest req, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            return (await _svc.AssignToolAsync(id, req, managerUserId, ct)) ? NoContent() : NotFound();
        }

        // [Authorize(Roles = "MANAGER")]
        [HttpPost("{id:long}/tools/return-all")]
        public async Task<IActionResult> ReturnAllTools(long id, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            var count = await _svc.ReturnAllToolsAsync(id, managerUserId, ct);
            return Ok(new { returned = count });
        }

        // SESSIONS
        // [Authorize(Roles = "MANAGER")]
        [HttpPost("{id:long}/work-session/force-end")]
        public async Task<IActionResult> ForceEndSession(long id, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            return (await _svc.ForceEndActiveWorkSessionAsync(id, managerUserId, ct)) ? NoContent() : NotFound();
        }

        // ADDRESSES (manager-only)
        // [Authorize(Roles = "MANAGER")]
        [HttpGet("{id:long}/addresses")]
        public async Task<IActionResult> GetAddresses(long id, CancellationToken ct)
            => Ok(await _svc.GetAddressesAsync(id, ct));

        // [Authorize(Roles = "MANAGER")]
        [HttpPost("{id:long}/addresses")]
        public async Task<IActionResult> AddAddress(long id, [FromBody] EmployeeAddressUpsertRequest req, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            var entityAddressId = await _svc.AddAddressAsync(id, req, managerUserId, ct);
            return Ok(new { entityAddressId });
        }

        // [Authorize(Roles = "MANAGER")]
        [HttpPut("{id:long}/addresses/{entityAddressId:long}")]
        public async Task<IActionResult> UpdateAddress(long id, long entityAddressId, [FromBody] EmployeeAddressUpsertRequest req, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            return (await _svc.UpdateAddressAsync(id, entityAddressId, req, managerUserId, ct)) ? NoContent() : NotFound();
        }

        // [Authorize(Roles = "MANAGER")]
        [HttpDelete("{id:long}/addresses/{entityAddressId:long}")]
        public async Task<IActionResult> DeleteAddress(long id, long entityAddressId, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            return (await _svc.DeleteAddressAsync(id, entityAddressId, managerUserId, ct)) ? NoContent() : NotFound();
        }

        // BANK ACCOUNTS (manager-only)
        // [Authorize(Roles = "MANAGER")]
        [HttpGet("{id:long}/bank-accounts")]
        public async Task<IActionResult> GetBankAccounts(long id, CancellationToken ct)
            => Ok(await _svc.GetBankAccountsAsync(id, ct));

        // [Authorize(Roles = "MANAGER")]
        [HttpPost("{id:long}/bank-accounts")]
        public async Task<IActionResult> AddBankAccount(long id, [FromBody] EmployeeBankAccountUpsertRequest req, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            var bankAccountId = await _svc.AddBankAccountAsync(id, req, managerUserId, ct);
            return Ok(new { bankAccountId });
        }

        // [Authorize(Roles = "MANAGER")]
        [HttpPut("{id:long}/bank-accounts/{bankAccountId:long}")]
        public async Task<IActionResult> UpdateBankAccount(long id, long bankAccountId, [FromBody] EmployeeBankAccountUpsertRequest req, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            return (await _svc.UpdateBankAccountAsync(id, bankAccountId, req, managerUserId, ct)) ? NoContent() : NotFound();
        }

        // [Authorize(Roles = "MANAGER")]
        [HttpDelete("{id:long}/bank-accounts/{bankAccountId:long}")]
        public async Task<IActionResult> DeleteBankAccount(long id, long bankAccountId, CancellationToken ct)
        {
            var managerUserId = GetUserIdOrThrow();
            return (await _svc.DeleteBankAccountAsync(id, bankAccountId, managerUserId, ct)) ? NoContent() : NotFound();
        }

        private long GetUserIdOrThrow()
        {
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(raw) || !long.TryParse(raw, out var id))
                throw new InvalidOperationException("Missing user id claim.");
            return id;
        }
        [HttpGet("{id:long}")]
public async Task<IActionResult> GetById(long id, CancellationToken ct)
{
    var emp = await _svc.GetByIdAsync(id, ct);

    if (emp == null)
        return NotFound();

    return Ok(emp);
}
    }
}
