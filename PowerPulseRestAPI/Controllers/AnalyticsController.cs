using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.BuissnesLogic.AnalyticsS;
using PowerPulseRestAPI.DTO.AnalyticsDto.Requests;
using PowerPulseRestAPI.DTO.AnalyticsDto.Responses;
using PowerPulseRestAPI.Reports;
using PowerPulseRestAPI.Reports.Models;

namespace PowerPulseRestAPI.Controllers;

[ApiController]
[Route("api/analytics")]
[Authorize(Roles = "ADMIN")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    
    private readonly IAnalyticsReportService  _analyticsReportService;

    public AnalyticsController(IAnalyticsService analyticsService, IAnalyticsReportService analyticsReportService)
    {
        _analyticsService = analyticsService;
        _analyticsReportService = analyticsReportService;
    }

    [HttpGet("overview")]
    public async Task<ActionResult<AnalyticsOverviewDto>> GetOverview([FromQuery] AnalyticsFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetOverviewAsync(filter, cancellationToken);
        
        return Ok(result);
    }

    [HttpGet("filter-options")]
    public async Task<ActionResult<AnalyticsFilterOptionsDto>> GetFilterOptionsAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetFilterOptionsAsync(cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost("report")]
    public async Task<IActionResult>
        GenerateReport(
            [FromBody] AnalyticsReportRequest request,
            CancellationToken cancellationToken = default)
    {
        var result =
            await _analyticsReportService
                .GenerateAsync(
                    request,
                    User.Identity?.Name,
                    cancellationToken);

        return File(
            result.Content,
            result.ContentType,
            result.FileName);
    }
}