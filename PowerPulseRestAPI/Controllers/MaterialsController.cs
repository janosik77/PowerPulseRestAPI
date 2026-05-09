using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.MaterialDto.Requests;
using PowerPulseRestAPI.DTO.MaterialDto.Responses;
using PowerPulseRestAPI.DTO.StockDto.Requests;
using PowerPulseRestAPI.Services.MaterialS;
using System.Security.Claims;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/materials")]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory(
            [FromBody] CreateMaterialCategoryDto dto,
            CancellationToken cancellationToken)
        {
            var id = await _materialService.CreateCategoryAsync(dto, cancellationToken);
            return Ok(new { id });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("categories/{id:long}")]
        public async Task<IActionResult> UpdateCategory(
            long id,
            [FromBody] UpdateMaterialCategoryDto dto,
            CancellationToken cancellationToken)
        {
            await _materialService.UpdateCategoryAsync(id, dto, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("categories/select-list")]
        [ProducesResponseType(typeof(IEnumerable<MaterialCategorySelectListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MaterialCategorySelectListDto>>> GetMaterialCategoriesSelectList()
        {
            var categories = await _materialService.GetMaterialCategoriesSelectListAsync();
            return Ok(categories);
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id, CancellationToken ct)
        {
            var result = await _materialService.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateMaterialDto dto,
            CancellationToken cancellationToken)
        {
            var id = await _materialService.CreateAsync(dto, cancellationToken);
            return Ok(new { id });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] UpdateMaterialDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _materialService.UpdateAsync(id, dto, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken cancellationToken)
        {
            await _materialService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpPost("movements")]
        public async Task<IActionResult> CreateMovement(
            [FromBody] CreateMaterialMovementDto dto,
            [FromQuery] long createdByUserId,
            CancellationToken cancellationToken)
        {
            await _materialService.CreateMovementAsync(dto, createdByUserId, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("stock")]
        public async Task<IActionResult> GetStock(CancellationToken cancellationToken)
        {
            var result = await _materialService.GetStockAsync(cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("projects/{projectId:long}/balance")]
        public async Task<IActionResult> GetProjectBalance(
            long projectId,
            CancellationToken cancellationToken)
        {
            var result = await _materialService.GetProjectBalanceAsync(projectId, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("projects/{projectId:long}/consumption")]
        public async Task<IActionResult> GetProjectConsumption(
            long projectId,
            CancellationToken cancellationToken)
        {
            var result = await _materialService.GetProjectConsumptionAsync(projectId, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpPost("low-stock-notes")]
        public async Task<IActionResult> CreateLowStockNote(
            [FromBody] CreateLowStockNoteDto dto,
            CancellationToken cancellationToken)
        {
            var currentEmployeeId = GetCurrentEmployeeId();

            var created = await _materialService.CreateLowStockNoteAsync(
                dto,
                currentEmployeeId,
                cancellationToken);

            return Ok(created);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("low-stock-notes")]
        public async Task<IActionResult> GetLowStockNotes(
            CancellationToken cancellationToken)
        {
            var currentEmployeeId = GetCurrentEmployeeId();
            var isAdmin = User.IsInRole("ADMIN");

            var result = await _materialService.GetLowStockNotesAsync(
                currentEmployeeId,
                isAdmin,
                cancellationToken);

            return Ok(result);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpGet("low-stock-notes/{id:long}")]
        public async Task<IActionResult> GetLowStockNoteById(
            long id,
            CancellationToken cancellationToken)
        {
            var currentEmployeeId = GetCurrentEmployeeId();
            var isAdmin = User.IsInRole("ADMIN");

            var result = await _materialService.GetLowStockNoteByIdAsync(
                id,
                currentEmployeeId,
                isAdmin,
                cancellationToken);

            return Ok(result);
        }


        [Authorize(Roles = "ADMIN, USER")]
        [HttpPut("low-stock-notes/{id:long}")]
        public async Task<IActionResult> UpdateLowStockNote(
            long id,
            [FromBody] UpdateLowStockNoteDto dto,
            CancellationToken cancellationToken)
        {
            var currentEmployeeId = GetCurrentEmployeeId();
            var isAdmin = User.IsInRole("ADMIN");

            var updated = await _materialService.UpdateLowStockNoteAsync(
                id,
                dto,
                currentEmployeeId,
                isAdmin,
                cancellationToken);

            return Ok(updated);
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpDelete("low-stock-notes/{id:long}")]
        public async Task<IActionResult> DeleteLowStockNote(
            long id,
            CancellationToken cancellationToken)
        {
            var currentEmployeeId = GetCurrentEmployeeId();
            var isAdmin = User.IsInRole("ADMIN");

            await _materialService.DeleteLowStockNoteAsync(
                id,
                currentEmployeeId,
                isAdmin,
                cancellationToken);

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
    }
}