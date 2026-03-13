using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.DTO.VehicleDto.Requests;
using PowerPulseRestAPI.DTO.VehicleDto.Responses;
using PowerPulseRestAPI.Services.Vehicles;

[ApiController]
[Route("api/vehicles")]
public sealed class VehiclesController : ControllerBase
{
    private readonly IVehicleService _service;
    public VehiclesController(IVehicleService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleListItemDto>>> GetList(CancellationToken ct)
        => Ok(await _service.GetListAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<VehicleDetailsDto>> GetDetails(long id, CancellationToken ct)
    {
        var dto = await _service.GetDetailsAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("{id:long}/load")]
    public async Task<ActionResult<VehicleLoadDto>> GetLoad(long id, CancellationToken ct)
    {
        var dto = await _service.GetLoadAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<VehicleCreatedDto>> Create([FromBody] CreateVehicleRequest req, CancellationToken ct)
    {
        var created = await _service.CreateAsync(req, ct);
        return CreatedAtAction(nameof(GetDetails), new { id = created.Id }, created);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateVehicleRequest req, CancellationToken ct)
    {
        var ok = await _service.UpdateAsync(id, req, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("{id:long}/status")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateVehicleStatusRequest req, CancellationToken ct)
    {
        var ok = await _service.UpdateStatusAsync(id, req.Status, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var ok = await _service.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
