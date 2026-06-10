using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/analytics")]
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
            var result = await _analyticsService.GetDashboardAsync(
                User,
                cancellationToken);

            return Ok(result);
        }
    }
}
