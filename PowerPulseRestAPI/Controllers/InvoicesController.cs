using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.InvoiceDto.Requests;
using PowerPulseRestAPI.Services.InvoiceS;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/invoices")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateInvoiceDto dto,
            [FromQuery] long createdByUserId,
            CancellationToken cancellationToken)
        {
            var created = await _invoiceService.CreateAsync(dto, createdByUserId, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken)
        {
            var result = await _invoiceService.GetListAsync(cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(
            long id,
            CancellationToken cancellationToken)
        {
            var result = await _invoiceService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("projects/{projectId:long}/material-select-options")]
        public async Task<IActionResult> GetMaterialSelectOptions(
            long projectId,
            [FromQuery] DateOnly billingPeriodStart,
            [FromQuery] DateOnly billingPeriodEnd,
            CancellationToken cancellationToken)
        {
            var result = await _invoiceService.GetMaterialSelectOptionsAsync(
                projectId,
                billingPeriodStart,
                billingPeriodEnd,
                cancellationToken);

            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] UpdateInvoiceDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _invoiceService.UpdateAsync(id, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{id:long}/status")]
        public async Task<IActionResult> UpdateStatus(
            long id,
            [FromBody] UpdateInvoiceStatusDto dto,
            CancellationToken cancellationToken)
        {
            var updated = await _invoiceService.UpdateStatusAsync(id, dto, cancellationToken);
            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken cancellationToken)
        {
            await _invoiceService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}