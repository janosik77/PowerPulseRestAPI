using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPulseRestAPI.Services.Uploads;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/uploads")]
    public class UploadsController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public UploadsController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("{category}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> Upload(
            string category,
            [FromForm] IFormFile file,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _fileStorageService.UploadAsync(file, category, cancellationToken);

                return Ok(new
                {
                    url = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
