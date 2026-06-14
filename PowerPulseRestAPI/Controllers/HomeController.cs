using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.BuissnesLogic.HomeS;


namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize(Roles = "ADMIN,USER")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard(
        
            CancellationToken cancellationToken)
        {
            
            var employeeIdValue = User.FindFirst("employeeId")?.Value;
            long.TryParse(employeeIdValue, out var employeeId);

            var result = await _homeService.GetDashboardAsync(
                employeeId,
                cancellationToken);
        
            return Ok(result);
        }
    }
}
