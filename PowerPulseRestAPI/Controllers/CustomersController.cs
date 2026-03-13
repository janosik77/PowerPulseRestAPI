using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.CustomerDto.Request;
using PowerPulseRestAPI.DTO.CustomerDto.Response;
using PowerPulseRestAPI.Services.Customers;
using System.Security.Claims;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public sealed class CustomersController : ControllerBase
    {
        private readonly ICustomerService _svc;

        public CustomersController(ICustomerService svc)
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

        // worker/manager
        [HttpGet]
        [Authorize]
        public Task<IReadOnlyList<CustomerListItemDto>> GetList(
            [FromQuery] string? q,
            [FromQuery] CustomerStatus? status,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
            => _svc.GetListAsync(q, status, skip, take, ct);

        // worker/manager
        [HttpGet("{id:long}")]
        [Authorize]
        public Task<CustomerPublicDetailsDto?> GetPublic(long id, CancellationToken ct)
            => _svc.GetPublicDetailsAsync(id, ct);

        // manager
        [HttpGet("{id:long}/details")]
        [Authorize(Roles = "Manager")]
        public Task<CustomerDetailsPrivateDto?> GetManagerDetails(long id, CancellationToken ct)
            => _svc.GetManagerDetailsAsync(id, ct);

        // manager
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<long>> Create([FromBody] CustomerCreateRequest req, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var id = await _svc.CreateAsync(req, userId, ct);
            return Ok(id);
        }

        // manager
        [HttpPut("{id:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(long id, [FromBody] CustomerUpdateRequest req, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var ok = await _svc.UpdateAsync(id, req, userId, ct);
            return ok ? NoContent() : NotFound();
        }

        // manager
        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var ok = await _svc.DeleteAsync(id, userId, ct);
            return ok ? NoContent() : NotFound();
        }

        // manager - contacts
        [HttpPost("{id:long}/contacts")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<long>> AddContact(long id, [FromBody] CustomerContactUpsertRequest req, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var newId = await _svc.AddContactAsync(id, req, userId, ct);
            return Ok(newId);
        }

        [HttpPut("{id:long}/contacts/{contactId:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateContact(long id, long contactId, [FromBody] CustomerContactUpsertRequest req, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var ok = await _svc.UpdateContactAsync(id, contactId, req, userId, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:long}/contacts/{contactId:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteContact(long id, long contactId, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var ok = await _svc.DeleteContactAsync(id, contactId, userId, ct);
            return ok ? NoContent() : NotFound();
        }

        // manager - addresses
        [HttpPost("{id:long}/addresses")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<long>> AddAddress(long id, [FromBody] CustomerAddressUpsertRequest req, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var newId = await _svc.AddAddressAsync(id, req, userId, ct);
            return Ok(newId);
        }

        [HttpPut("{id:long}/addresses/{entityAddressId:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateAddress(long id, long entityAddressId, [FromBody] CustomerAddressUpsertRequest req, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var ok = await _svc.UpdateAddressAsync(id, entityAddressId, req, userId, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:long}/addresses/{entityAddressId:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteAddress(long id, long entityAddressId, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var ok = await _svc.DeleteAddressAsync(id, entityAddressId, userId, ct);
            return ok ? NoContent() : NotFound();
        }

        // manager - notes
        [HttpPost("{id:long}/notes")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<long>> AddNote(long id, [FromBody] CustomerNoteCreateRequest req, CancellationToken ct)
        {
            var userId = GetUserIdOrThrow();
            var newId = await _svc.AddNoteAsync(id, req, userId, ct);
            return Ok(newId);
        }

        // manager - invoices
        [HttpGet("{id:long}/invoices")]
        [Authorize(Roles = "Manager")]
        public Task<IReadOnlyList<CustomerInvoiceListItemDto>> GetInvoices(
            long id,
            [FromQuery] InvoiceStatus? status,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
            => _svc.GetInvoicesAsync(id, status, from, to, skip, take, ct);
    }
}
