// Controllers/MaterialsController.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.MaterialDto.Request;
using PowerPulseRestAPI.DTO.MaterialDto.Response;
using PowerPulseRestAPI.Services.MaterialService;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/materials")]
    [Authorize]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MaterialDto>>> GetAll([FromQuery] bool includeArchived = false, CancellationToken ct = default)
        {
            return Ok(await _materialService.GetMaterialsAsync(includeArchived, ct));
        }

        [HttpGet("{materialId:long}")]
        public async Task<ActionResult<MaterialDto>> GetById(long materialId, CancellationToken ct = default)
        {
            var result = await _materialService.GetMaterialByIdAsync(materialId, ct);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<MaterialDto>> Create([FromBody] CreateMaterialRequest request, CancellationToken ct = default)
        {
            var result = await _materialService.CreateMaterialAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { materialId = result.Id }, result);
        }

        [HttpPut("{materialId:long}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<MaterialDto>> Update(long materialId, [FromBody] UpdateMaterialRequest request, CancellationToken ct = default)
        {
            var result = await _materialService.UpdateMaterialAsync(materialId, request, ct);
            return Ok(result);
        }

        [HttpDelete("{materialId:long}/soft")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> SoftDelete(long materialId, CancellationToken ct = default)
        {
            await _materialService.SoftDeleteMaterialAsync(materialId, ct);
            return NoContent();
        }

        [HttpPost("{materialId:long}/restore")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Restore(long materialId, CancellationToken ct = default)
        {
            await _materialService.RestoreMaterialAsync(materialId, ct);
            return NoContent();
        }

        [HttpDelete("{materialId:long}/hard")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> HardDelete(long materialId, CancellationToken ct = default)
        {
            await _materialService.HardDeleteMaterialAsync(materialId, ct);
            return NoContent();
        }

        [HttpGet("transfer-context/warehouse")]
        public async Task<ActionResult<MaterialTransferContextDto>> GetWarehouseContext(CancellationToken ct = default)
        {
            return Ok(await _materialService.GetWarehouseContextAsync(ct));
        }

        [HttpGet("transfer-context/projects")]
        public async Task<ActionResult<List<MaterialProjectLookupDto>>> GetProjects(CancellationToken ct = default)
        {
            return Ok(await _materialService.GetAvailableProjectsAsync(GetCurrentUserId(), IsManager(), ct));
        }

        [HttpGet("transfer-context/project/{projectId:long}")]
        public async Task<ActionResult<MaterialTransferContextDto>> GetProjectContext(long projectId, CancellationToken ct = default)
        {
            return Ok(await _materialService.GetProjectContextAsync(projectId, GetCurrentUserId(), IsManager(), ct));
        }

        [HttpPost("transfer/commit")]
        public async Task<IActionResult> CommitTransfer([FromBody] CommitMaterialTransferRequest request, CancellationToken ct = default)
        {
            await _materialService.CommitTransferAsync(request, GetCurrentUserId(), IsManager(), ct);
            return NoContent();
        }

        [HttpGet("inventory/project/{projectId:long}")]
        public async Task<ActionResult<List<ProjectInventoryItemDto>>> GetInventoryPreview(long projectId, CancellationToken ct = default)
        {
            return Ok(await _materialService.GetProjectInventoryPreviewAsync(projectId, GetCurrentUserId(), IsManager(), ct));
        }

        [HttpPost("inventory/submit")]
        public async Task<IActionResult> SubmitInventory([FromBody] SubmitProjectInventoryRequest request, CancellationToken ct = default)
        {
            await _materialService.SubmitProjectInventoryAsync(request, GetCurrentUserId(), IsManager(), ct);
            return NoContent();
        }

        [HttpGet("consumes/project/{projectId:long}")]
        public async Task<ActionResult<List<MaterialProjectConsumeDto>>> GetProjectConsumes(long projectId, CancellationToken ct = default)
        {
            return Ok(await _materialService.GetProjectConsumeHistoryAsync(projectId, GetCurrentUserId(), IsManager(), ct));
        }

        [HttpGet("{materialId:long}/movements")]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetMaterialMovements(long materialId, CancellationToken ct = default)
        {
            return Ok(await _materialService.GetMaterialMovementHistoryAsync(materialId, ct));
        }

        private long GetCurrentUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return long.Parse(value!);
        }

        private bool IsManager()
        {
            return User.IsInRole("Manager");
        }
    }
}
