using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.DTO.Auth.Request;
using PowerPulseRestAPI.DTO.Auth.Response;
using PowerPulseRestAPI.Services.Security;

namespace PowerPulseRestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly PowerPulseContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(
            PowerPulseContext context,
            IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {

            //var pass = BCrypt.Net.BCrypt.HashPassword("12345");

            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Login i hasło są wymagane." });
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Person)
                    .ThenInclude(p => p.Employee)
                .FirstOrDefaultAsync(u => u.Login == request.Login && !u.IsDeleted);

            if (user == null || !user.IsActive)
            {
                return Unauthorized(new { message = "Nieprawidłowy login lub hasło." });
            }

            var passwordIsValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!passwordIsValid)
            {
                return Unauthorized(new { message = "Nieprawidłowy login lub hasło." });
            }

            var roleName = user.Role?.Name ?? "USER";

            var token = _jwtTokenService.GenerateToken(
                user.Id,
                user.Login,
                user.Email,
                roleName,
                user.Person!.Employee!.Id
            );

            user.LastLoginAt = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();

            var fullName = user.Person != null
                ? $"{user.Person.FirstName} {user.Person.LastName}".Trim()
                : user.Login;

            return Ok(new LoginResponseDto
            {
                Token = token,
                User = new
                {
                    id = user.Id,
                    login = user.Login,
                    email = user.Email,
                    role = roleName,
                    fullName,
                    employeeId = user.Person!.Employee.Id
                }
            });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role)
                            .Select(r => r.Value)
                            .ToList();

            return Ok(new
            {
                id = userId,
                email = email,
                roles = roles
            });
        }
    }
}
